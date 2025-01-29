using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace int_db.scripts
{
    /// <summary>
    /// Classe permettant de :
    /// - Se connecter à un serveur MinIO
    /// - Lister les buckets
    /// - Télécharger un fichier
    /// - Téléverser un fichier
    /// - Fermer la connexion au serveur MinIO
    /// </summary>
    public class MinioClientWrapper
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
        /// Ferme la connexion au serveur MinIO.
        /// </summary>
        public void CloseConnection()
        {
            // Pas de méthode explicite pour fermer la connexion dans MinIO SDK
            _minioClient = null;
            Console.WriteLine("Connexion à MinIO fermée.");
        }

        /// <summary>
        /// Liste les buckets disponibles sur le serveur MinIO et retourne les résultats sous forme de liste.
        /// </summary>
        /// <returns>Liste des noms de buckets.</returns>
        public async Task<List<string>> ListBucketsAsync()
        {
            var bucketNames = new List<string>();

            try
            {
                var buckets = await _minioClient.ListBucketsAsync();

                if (buckets.Buckets.Count == 0)
                {
                    Console.WriteLine("Aucun bucket trouvé.");
                    return bucketNames;
                }

                foreach (var bucket in buckets.Buckets)
                {
                    bucketNames.Add(bucket.Name);
                }
            }
            catch (MinioException ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des buckets : {ex.Message}");
            }

            return bucketNames;
        }

        /// <summary>
        /// Téléverse un fichier dans un bucket spécifié.
        /// </summary>
        /// <param name="bucketName">Nom du bucket.</param>
        /// <param name="objectName">Nom de l'objet.</param>
        /// <param name="filePath">Chemin du fichier à téléverser.</param>
        /// <returns>Message de succès ou d'erreur.</returns>
        public async Task<string> UploadFileAsync(string bucketName, string objectName, string filePath)
        {
            try
            {
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFileName(filePath));

                return $"Fichier {filePath} téléversé dans le bucket {bucketName} sous le nom {objectName}.";
            }
            catch (MinioException ex)
            {
                return $"Erreur lors de l'upload : {ex.Message}";
            }
        }

        /// <summary>
        /// Télécharge un fichier depuis un bucket spécifié.
        /// </summary>
        /// <param name="bucketName">Nom du bucket.</param>
        /// <param name="objectName">Nom de l'objet.</param>
        /// <param name="filePath">Chemin de destination du fichier.</param>
        /// <returns>Message de succès ou d'erreur.</returns>
        public async Task<string> DownloadFileAsync(string bucketName, string objectName, string filePath)
        {
            try
            {
                await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFile(filePath));

                return $"Fichier {objectName} téléchargé depuis le bucket {bucketName} vers {filePath}.";
            }
            catch (MinioException ex)
            {
                return $"Erreur lors du téléchargement : {ex.Message}";
            }
        }
    }
}