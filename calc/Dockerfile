# Utiliser l'image Python 3.13
FROM python:3.13

# Définir le répertoire de travail
WORKDIR /usr/src/app

# Copier et installer les dépendances
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# Copier le code source
COPY src ./src

# Exposer le port pour l'application
EXPOSE 8080

# Lancer l'application avec uvicorn
CMD ["python", "src/app/calculs.py"]