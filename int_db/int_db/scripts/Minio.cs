using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace int_db.scripts
{
    /// <summary>
    /// Classe permettant de :
    /// - Se connecter à un serveur MinIO
    /// - Lister les buckets
    /// - Téléverser un fichier
    /// - Fermer la connexion au serveur MinIO
    /// </summary>
    public class MinioClientWrapper
    {
        private IMinioClient _client;

        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }

        /// <summary>
        /// Initialise la connexion avec les paramètres spécifiés.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                _client = new MinioClient()
                    .WithEndpoint(Endpoint)
                    .WithCredentials(AccessKey, SecretKey)
                    .Build();
                Console.WriteLine($"Connexion à MinIO réussie avec {Endpoint}.");
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de la connexion à MinIO : {ex.Message}");
            }
        }
        
        /// <summary>
        /// Vérifie si la connexion à MinIO est ouverte.
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return _client != null;
        }

        /// <summary>
        /// Ferme la connexion à MinIO.
        /// </summary>
        public void CloseConnection()
        {
            _client.Dispose();
            Console.WriteLine("Connexion à MinIO fermée.");
        }

        /// <summary>
        /// Crée un bucket s'il n'existe pas.
        /// </summary>
        public bool CreateBucket(string bucketName)
        {
            try
            {
                bool bucketExists = _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)).Result;
                if (!bucketExists)
                {
                    _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName)).Wait();
                    Console.WriteLine($"Bucket '{bucketName}' créé avec succès.");
                }
                else
                {
                    Console.WriteLine($"Bucket '{bucketName}' existe déjà.");
                }
                
                return true;
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de la création du bucket '{bucketName}' : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Liste les buckets disponibles.
        /// </summary>
        /// <returns>Liste des buckets.</returns>
        public async Task<List<string>> ListBuckets()
        {
            try
            {
                var list = await _client.ListBucketsAsync().ConfigureAwait(false);
                
                List<string> buckets = new List<string>();
                
                foreach (var bucket in list.Buckets)
                {
                    buckets.Add(bucket.Name + " " + bucket.CreationDate);
                }
                
                return buckets;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
                return new List<string>();
            }
        }
        
        /// <summary>
        /// Vérifie si un objet existe dans un bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public async Task<bool> ObjectExists(string bucketName, string objectName)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                var result = await _client.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
                return result != null;
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de la vérification de l'objet '{objectName}' dans le bucket '{bucketName}' : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stocke une suite de Syracuse dans MinIO sous forme de fichier texte.
        /// </summary>
        public async Task<bool> UploadSyracuse(string bucketName, string objectName, string syracuse)
        {
            try
            { 
                // Convertir en stream
                byte[] byteArray = Encoding.UTF8.GetBytes(syracuse);
                using MemoryStream stream = new MemoryStream(byteArray);

                // Envoi à MinIO
                await _client.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(byteArray.Length)
                    .WithContentType("text/plain"));

                Console.WriteLine($"Suite de Syracuse stockée dans '{bucketName}/{objectName}'.");
                
                return true;
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors du téléversement de la suite de Syracuse : {ex.Message}");
                return false;
            }
        }
    }
}
