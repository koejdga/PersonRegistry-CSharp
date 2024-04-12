using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows;
using Person.Models;

namespace Person.Repositories
{
    class FileRepository
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonAppCSharp");
        private static readonly string IsInitialized = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonAppCSharp", "isInitialized");


        public FileRepository()
        {
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
        }

        public void AddOrUpdate(DbPersonModel obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, obj.Email), false))
            {
                sw.Write(stringObj);
            }
        }

        public async Task AddOrUpdateAsync(DbPersonModel obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, obj.Email), false))
            {
                await sw.WriteAsync(stringObj);
            }
        }

        public void Delete(string id)
        {
            var filePath = Path.Combine(BaseFolder, id);
            if (!File.Exists(filePath)) return;
            try
            {
                File.Delete(filePath);
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        public void Initialize(string filepath)
        {
            using (File.Create(filepath)) {}
        }

        public DbPersonModel Get(string login)
        {
            string stringObj = null;
            string filePath = Path.Combine(BaseFolder, login);

            if (!File.Exists(filePath) || Directory.EnumerateFiles(BaseFolder).Count() == 0)
                return null;

            using (StreamReader sw = new StreamReader(filePath))
            {
                stringObj = sw.ReadToEnd();
            }

            return JsonSerializer.Deserialize<DbPersonModel>(stringObj);
        }

        public async Task<DbPersonModel> GetAsync(string login)
        {
            string stringObj = null;
            string filePath = Path.Combine(BaseFolder, login);

            if (!File.Exists(filePath))
                return null;

            using (StreamReader sw = new StreamReader(filePath))
            {
                stringObj = await sw.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<DbPersonModel>(stringObj);
        }

        public List<DbPersonModel> GetAll()
        {
            var res = new List<DbPersonModel>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                if (file == IsInitialized)
                    continue;

                string stringObj = null;

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = sw.ReadToEnd();
                }

                res.Add(JsonSerializer.Deserialize<DbPersonModel>(stringObj));
            }

            return res;
        }

        public void DeleteAll()
        {
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message, "Error");
                }
            }
        }
    }
}
