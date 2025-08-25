node {
  stage('SCM') {
    checkout scm
  }
  stage('SonarQube Analysis') {
    tools {
      // Define the .NET SDK tool, configured in Jenkins global tool configuration
      dotnet 'built-in' 
    }
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
