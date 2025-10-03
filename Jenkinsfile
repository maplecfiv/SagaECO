node {
  stage('SCM') {
    checkout scm
  }
  stage('SonarQube Analysis') {
    def scannerHome = tool 'SonarScanner for MSBuild'
    withSonarQubeEnv() {
      withEnv(['DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1']) {
        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"maplecfiv_SagaECO_AZjVhprtx_QsD-k0ns95\""
        sh "dotnet build PlutoProject/SagaECO.sln"
        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
      }

    }
  }
}
