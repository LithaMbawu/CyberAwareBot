using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MySqlConnector;

namespace CybersecurityChatbot
{
    public sealed class CyberSecurityTaskStore
    {
        private readonly string _jsonFilePath = Path.Combine(AppContext.BaseDirectory, "cybersecurity_tasks.json");
        private readonly string? _mysqlConnectionString;
        private bool _useMySql;

        public CyberSecurityTaskStore()
        {
            _mysqlConnectionString = Environment.GetEnvironmentVariable("CYBERAWAREBOT_MYSQL_CONNECTION");
            _useMySql = !string.IsNullOrWhiteSpace(_mysqlConnectionString);

            if (_useMySql)
            {
                try
                {
                    EnsureMySqlSchema();
                }
                catch
                {
                    _useMySql = false;
                }
            }
        }

        public List<CyberSecurityTask> GetAllTasks()
        {
            var tasks = _useMySql ? LoadFromMySql() : LoadFromJson();
            return tasks.OrderByDescending(task => task.CreatedAt).ToList();
        }

        public CyberSecurityTask AddTask(CyberSecurityTask task)
        {
            List<CyberSecurityTask> tasks = GetAllTasks();
            task.Id = tasks.Count == 0 ? 1 : tasks.Max(item => item.Id) + 1;
            task.CreatedAt = task.CreatedAt == default ? DateTime.Now : task.CreatedAt;
            tasks.Add(task);
            Save(tasks);
            return task;
        }

        public bool UpdateTask(CyberSecurityTask task)
        {
            List<CyberSecurityTask> tasks = GetAllTasks();
            int index = tasks.FindIndex(item => item.Id == task.Id);
            if (index < 0)
            {
                return false;
            }

            tasks[index] = task;
            Save(tasks);
            return true;
        }

        public bool DeleteTask(int taskId)
        {
            List<CyberSecurityTask> tasks = GetAllTasks();
            int removed = tasks.RemoveAll(task => task.Id == taskId);
            if (removed == 0)
            {
                return false;
            }

            Save(tasks);
            return true;
        }

        private void Save(List<CyberSecurityTask> tasks)
        {
            if (_useMySql)
            {
                SaveToMySql(tasks);
                return;
            }

            SaveToJson(tasks);
        }

        private void EnsureMySqlSchema()
        {
            using var connection = new MySqlConnection(_mysqlConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
CREATE TABLE IF NOT EXISTS cybersecurity_tasks (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    ReminderText VARCHAR(255) NULL,
    ReminderDate DATETIME NULL,
    IsCompleted TINYINT(1) NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL
);";
            command.ExecuteNonQuery();
        }

        private List<CyberSecurityTask> LoadFromMySql()
        {
            var tasks = new List<CyberSecurityTask>();

            try
            {
                using var connection = new MySqlConnection(_mysqlConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Title, Description, ReminderText, ReminderDate, IsCompleted, CreatedAt FROM cybersecurity_tasks ORDER BY CreatedAt DESC;";

                using var reader = command.ExecuteReader();
                int idOrdinal = reader.GetOrdinal("Id");
                int titleOrdinal = reader.GetOrdinal("Title");
                int descriptionOrdinal = reader.GetOrdinal("Description");
                int reminderTextOrdinal = reader.GetOrdinal("ReminderText");
                int reminderDateOrdinal = reader.GetOrdinal("ReminderDate");
                int isCompletedOrdinal = reader.GetOrdinal("IsCompleted");
                int createdAtOrdinal = reader.GetOrdinal("CreatedAt");

                while (reader.Read())
                {
                    tasks.Add(new CyberSecurityTask
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Title = reader.GetString(titleOrdinal),
                        Description = reader.GetString(descriptionOrdinal),
                        ReminderText = reader.IsDBNull(reminderTextOrdinal) ? null : reader.GetString(reminderTextOrdinal),
                        ReminderDate = reader.IsDBNull(reminderDateOrdinal) ? null : reader.GetDateTime(reminderDateOrdinal),
                        IsCompleted = reader.GetBoolean(isCompletedOrdinal),
                        CreatedAt = reader.GetDateTime(createdAtOrdinal)
                    });
                }
            }
            catch
            {
                return LoadFromJson();
            }

            return tasks;
        }

        private void SaveToMySql(List<CyberSecurityTask> tasks)
        {
            try
            {
                using var connection = new MySqlConnection(_mysqlConnectionString);
                connection.Open();

                using var transaction = connection.BeginTransaction();

                using (var deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.Transaction = transaction;
                    deleteCommand.CommandText = "DELETE FROM cybersecurity_tasks;";
                    deleteCommand.ExecuteNonQuery();
                }

                foreach (CyberSecurityTask task in tasks)
                {
                    using var insertCommand = connection.CreateCommand();
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandText = @"
INSERT INTO cybersecurity_tasks (Id, Title, Description, ReminderText, ReminderDate, IsCompleted, CreatedAt)
VALUES (@Id, @Title, @Description, @ReminderText, @ReminderDate, @IsCompleted, @CreatedAt);";

                    insertCommand.Parameters.AddWithValue("@Id", task.Id);
                    insertCommand.Parameters.AddWithValue("@Title", task.Title);
                    insertCommand.Parameters.AddWithValue("@Description", task.Description);
                    insertCommand.Parameters.AddWithValue("@ReminderText", (object?)task.ReminderText ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@ReminderDate", (object?)task.ReminderDate ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
                    insertCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                SaveToJson(tasks);
                _useMySql = false;
            }
        }

        private List<CyberSecurityTask> LoadFromJson()
        {
            try
            {
                if (!File.Exists(_jsonFilePath))
                {
                    return new List<CyberSecurityTask>();
                }

                string json = File.ReadAllText(_jsonFilePath);
                return JsonSerializer.Deserialize<List<CyberSecurityTask>>(json) ?? new List<CyberSecurityTask>();
            }
            catch
            {
                return new List<CyberSecurityTask>();
            }
        }

        private void SaveToJson(List<CyberSecurityTask> tasks)
        {
            try
            {
                string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, json);
            }
            catch
            {
            }
        }
    }
}
