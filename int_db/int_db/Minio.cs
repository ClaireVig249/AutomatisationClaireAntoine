using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;

namespace IntDB
{
    /// <summary>
    /// Classe permettant de :
    /// - Se connecter à un serveur MinIO
    /// - Lister les buckets
    /// - Télécharger un fichier
    /// - Téléverser un fichier
    /// - Fermer la connexion au serveur MinIO
    /// </summary>
    internal class MinioClientWrapper
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Port { get; set; } = "9000"; // Port par défaut pour MinIO

        private MinioClient _minioClient;

        /// <summary>
        /// Initialise la connexion avec les paramètres spécifiés.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                // Initialisation du client MinIO
                _minioClient = (MinioClient)new MinioClient()
                    .WithEndpoint(Host, int.Parse(Port))
                    .WithCredentials(AccessKey, SecretKey)
                    .Build();

                Console.WriteLine($"Connexion à MinIO réussie avec {Host}:{Port}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion à MinIO : {ex.Message}");
            }
        }

        /// <summary>
        /// Liste les buckets disponibles sur le serveur MinIO.
        /// </summary>
        /// <returns></returns>
        public async Task ListBucketsAsync()
        {
            try
            {
                var buckets = await _minioClient.ListBucketsAsync();
                
                if (buckets.Buckets.Count == 0)
                {
                    Console.WriteLine("Aucun bucket trouvé.");
                    return;
                }
                
                Console.WriteLine("Liste des buckets :");
                foreach (var bucket in buckets.Buckets)
                {
                    Console.WriteLine(bucket.Name);
                }
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des buckets : {ex.Message}");
            }
        }

        /// <summary>
        /// Ferme la connexion au serveur MinIO.
        /// </summary>
        public void CloseConnection()
        {
            // Pas de méthode explicite pour fermer la connexion dans MinIO SDK
            _minioClient = null;
            Console.WriteLine("Connexion à MinIO fermée.");
        }

        /// <summary>
        /// Téléverse un fichier dans un bucket spécifié.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task UploadFileAsync(string bucketName, string objectName, string filePath)
        {
            try
            {
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFileName(filePath));

                Console.WriteLine($"Fichier {filePath} téléchargé dans le bucket {bucketName} sous le nom {objectName}.");
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de l'upload : {ex.Message}");
            }
        }

        /// <summary>
        /// Télécharge un fichier depuis un bucket spécifié.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string bucketName, string objectName, string filePath)
        {
            try
            {
                await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFile(filePath));

                Console.WriteLine($"Fichier {objectName} téléchargé depuis le bucket {bucketName} vers {filePath}.");
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors du téléchargement : {ex.Message}");
            }
        }
    }
}