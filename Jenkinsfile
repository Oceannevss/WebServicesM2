pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                // Récupérer le code depuis le dépôt Git
                git 'https://github.com/Oceannevss/WebServicesM2.git'
            }
        }
        stage('Restore') {
            steps {
                // Restaurer les dépendances du projet dotnet
                sh 'dotnet restore'
            }
        }
        stage('Build') {
            steps {
                // Compiler le projet dotnet
                sh 'dotnet build --configuration Release'
            }
        }
        stage('Publish') {
            steps {
                // Publier l'application dotnet
                sh 'dotnet publish --configuration Release --output publishOutput'
            }
        }
        stage('Containerize') {
            steps {
                // Construction et publication de l'image Docker
                sh 'docker build -t docker-image:version .'
                sh 'docker push docker-image:version'
            }
        }
    }
}
