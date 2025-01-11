FROM php:8.3-apache

# PHP
RUN apt-get update && apt-get install -y libpq-dev
RUN docker-php-ext-install pdo pdo_pgsql

# Apache
RUN a2enmod rewrite

# Définir le répertoire de travail
WORKDIR /var/www/html

# Copier le code source
COPY . .

# Exposer le port pour l'application
EXPOSE 80