pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                // Récupérer le code depuis le dépôt Git
                git 'https://github.com/Oceannevss/WebServicesM2.git'
            }
        }
        stage('Build') {
            steps {
                // Compilation en release du projet (à adapter selon votre technologie)
                sh './gradlew clean assembleRelease' // Exemple avec Gradle
            }
        }
        stage('Publish') {
            steps {
                // Publier l'application (à adapter selon votre technologie)
                // Exemple avec Maven pour publier dans un repository Maven
                sh './mvnw deploy' 

                // Assurez-vous d'avoir configuré vos informations d'authentification pour publier dans Nexus
            }
        }
        stage('Containerize') {
            steps {
                // Ajouter le résultat dans Nexus (après conteneurisation)
                sh 'docker build -t docker-image:version .' // Exemple de construction de l'image Docker
                sh 'docker push docker-image:version' // Exemple de publication de l'image dans un registre Docker (Nexus)
            }
        }
    }
}