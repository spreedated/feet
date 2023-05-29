using FeetScraper.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FeetFinder.Service
{
    internal class DownloadService : ICollection<FootPicture>
    {
        private List<FootPicture> footPictures;
        private readonly string path = null;
        private bool processing;

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public DownloadService(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path cannot be empty", nameof(path));
            }

            this.footPictures = new();
            this.path = path;
        }

        public Task LoadAsync()
        {
            return Task.Factory.StartNew(() => this.Load());
        }

        public void Load()
        {
            if (processing)
            {
                return;
            }

            processing = true;

            string json = null;

            this.Touch();

            using (FileStream fs = File.Open(this.path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader r = new(fs))
                {
                    json = r.ReadToEnd();
                }
            }

            if (!string.IsNullOrEmpty(json))
            {
                this.footPictures = JsonConvert.DeserializeObject<List<FootPicture>>(json);
            }


            processing = false;
        }

        public Task SaveAsync()
        {
            return Task.Factory.StartNew(() => this.Save());
        }

        public void Save()
        {
            if (processing)
            {
                return;
            }

            processing = true;

            this.Touch();

            using (FileStream fs = File.Open(this.path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter w = new(fs))
                {
                    w.Write(JsonConvert.SerializeObject(this.footPictures));
                }
            }

            processing = false;
        }

        private void Touch()
        {
            if (!File.Exists(this.path))
            {
                File.Create(this.path).Dispose();
            }
        }

        #region ICollection
        public void Add(FootPicture item)
        {
            if (item != null && !this.footPictures.Any(x => x.Id == item.Id))
            {
                this.footPictures.Add(item);
            }
        }

        public void Clear()
        {
            this.footPictures.Clear();
            this.Save();
        }

        public bool Contains(FootPicture item)
        {
            return this.footPictures.Contains(item);
        }

        public void CopyTo(FootPicture[] array, int arrayIndex)
        {
            this.footPictures.CopyTo(array, arrayIndex);
        }

        public bool Remove(FootPicture item)
        {
            return this.footPictures.Remove(item);
        }

        public IEnumerator<FootPicture> GetEnumerator()
        {
            return this.footPictures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
