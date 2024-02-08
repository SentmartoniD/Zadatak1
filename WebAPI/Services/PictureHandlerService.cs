using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Services
{
    public class PictureHandlerService : IPictureHandler
    {
        private readonly string pathToPicturesFolder;

        public PictureHandlerService(IWebHostEnvironment environment)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\"));
            pathToPicturesFolder = targetDirectory + "Pictures";
        }
        public async Task<bool> DeleteByName(string pictureName)
        {
            string path = Path.Combine(pathToPicturesFolder, pictureName);
            // CHECK IF FILE EXISTS
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetAll()
        {
            // GETS FULL PATHS TO JPG PICTURES
            string[] picturesJPG = Directory.GetFiles(pathToPicturesFolder, "*.jpg");
            // GETS FULL PATHS TO PNG PICTURES
            string[] picturesPng = Directory.GetFiles(pathToPicturesFolder, "*.png");

            string[] resWithFullPaths = picturesJPG.Concat(picturesPng).ToArray();
            List<string> res = new List<string>();
            // REMOVES THE PATH PART
            foreach (string s in resWithFullPaths)
                res.Add(s.Substring(pathToPicturesFolder.Length + 1));

            // SORT LIST
            res.Sort();
            return res;
        }

        public async Task<byte[]> GetByName(string pictureName)
        {

            string path = Path.Combine(pathToPicturesFolder, pictureName);
            byte[] imageBytes = null;

            if (File.Exists(path))
            {
                using (Bitmap image = new Bitmap(path))
                {


                    using (MemoryStream stream = new MemoryStream())
                    {

                        ImageFormat format;
                        if (Path.GetExtension(path).ToLower() == ".png")
                        {
                            format = ImageFormat.Png;
                            image.Save(stream, format);
                        }
                        else if (Path.GetExtension(path).ToLower() == ".jpg" || Path.GetExtension(path).ToLower() == ".jpeg")
                        {
                            format = ImageFormat.Jpeg;
                            image.Save(stream, format);
                        }

                        // CONVERT BITMAP TO BYTE ARRAY
                        imageBytes = stream.ToArray();

                    }
                }
            }

            return imageBytes;
        }

        public async Task<bool> Save(IFormFile picture)
        {
            var fileName = Path.GetFileName(picture.FileName);
            var fileExtension = Path.GetExtension(fileName);


            var uniqueFileName = CreateHash256(fileName) + fileExtension;

            // PATH FOR THE FILE
            var filePath = Path.Combine(pathToPicturesFolder, uniqueFileName);

            if (File.Exists(filePath))
                return false;

            // SAVE TO FOLDER PICTURES
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }


            return true;
        }

        // CREATE A HASH FROM A STRING
        private string CreateHash256(string name)
        {
            byte[] nameInBytes = Encoding.UTF8.GetBytes(name);

            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] hashBytes = sha256.ComputeHash(nameInBytes);


                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                string hashString = stringBuilder.ToString();

                return hashString;
            }
        }

       
    }
}
