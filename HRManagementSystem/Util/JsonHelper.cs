using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace HRManagementSystem.Util
{
    public static class JsonHelper
    {
        /// <summary>
        /// Validates if the given file contains valid JSON
        /// </summary>
        public static bool ValidateJsonFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File does not exist: {filePath}");
                    return false;
                }

                string jsonContent = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine($"File is empty: {filePath}");
                    return false;
                }

                // Try to parse the JSON
                using (JsonDocument.Parse(jsonContent))
                {
                    Console.WriteLine($"JSON is valid: {filePath}");
                    return true;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Invalid JSON in {filePath}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating JSON file {filePath}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Attempts to deserialize a JSON file to the specified type
        /// </summary>
        public static bool TryDeserialize<T>(string filePath, out T result) where T : class
        {
            result = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File does not exist: {filePath}");
                    return false;
                }

                string jsonContent = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Console.WriteLine($"File is empty: {filePath}");
                    return false;
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                result = JsonSerializer.Deserialize<T>(jsonContent, options);
                return result != null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize {filePath}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file {filePath}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Prints JSON structure for debugging
        /// </summary>
        public static void DebugPrintJson<T>(T obj) where T : class
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(obj, options);
                Console.WriteLine("JSON Structure:");
                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing object: {ex.Message}");
            }
        }
    }
}
