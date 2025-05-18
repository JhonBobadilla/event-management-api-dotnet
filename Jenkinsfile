pipeline {
    agent any

    environment {
        DOTNET_VERSION = '8.0.x'
    }

    stages {
        stage('Checkout') {
            steps {
                
                checkout scm
            }
        }
        stage('Setup .NET') {
            steps {
                sh 'wget https://dot.net/v1/dotnet-install.sh'
                sh 'chmod +x dotnet-install.sh'
                sh './dotnet-install.sh --version 8.0.100'
                sh 'export PATH=$PATH:$HOME/.dotnet'
            }
        }
        stage('Restore') {
            steps {
                sh '$HOME/.dotnet/dotnet restore'
            }
        }
        stage('Build') {
            steps {
                sh '$HOME/.dotnet/dotnet build --configuration Release --no-restore'
            }
        }
        stage('Test') {
            steps {
                sh '$HOME/.dotnet/dotnet test --configuration Release --no-build'
            }
        }
        stage('Publish') {
            steps {
                sh '$HOME/.dotnet/dotnet publish --configuration Release -o publish'
                archiveArtifacts artifacts: 'publish/**', fingerprint: true
            }
        }
      
    }

    post {
        always {
            cleanWs()
        }
        failure {
            mail to: 'correo@correo.com',
                 subject: "Pipeline Failed: ${env.JOB_NAME} [${env.BUILD_NUMBER}]",
                 body: "Check Jenkins for details."
        }
    }
}
