# Utiliser l'image officielle de PHP 8.3 avec Apache
FROM php:8.3-apache

# Installer les dépendances
RUN apt-get update && apt-get install -y libpq-dev
RUN docker-php-ext-install pdo pdo_pgsql

# Activer le module rewrite
RUN a2enmod rewrite

# Définir le répertoire de travail
WORKDIR /var/www/html

# Copier le code source
COPY ./src .

# Exposer le port pour l'application
EXPOSE 80