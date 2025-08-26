pipeline {
  agent any
  environment {

  }
  tools {
    dotnetsdk 'built-in'
  }
  stages {
    stage('SCM') {
      steps{
        checkout scm
      }
    }
    stage('SonarQube Analysis') {
      steps{
        def scannerHome = tool 'SonarScanner for MSBuild'
        withSonarQubeEnv(){
          withDotNet() {
            sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"maplecfiv_SagaECO_AZjVhprtx_QsD-k0ns95\""
            sh "dotnet build"
            sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
          }
        }
      }
      
    }
  }

}
