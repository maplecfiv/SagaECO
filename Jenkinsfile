node {
  stage('SCM') {
    checkout scm
  }
  stage('SonarQube Analysis') {
    def scannerHome = tool 'SonarScanner for MSBuild'
    withSonarQubeEnv(){
      withDotNet([string(sdk: '6.0.428')]) {
        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"maplecfiv_SagaECO_AZjVhprtx_QsD-k0ns95\""
        sh "dotnet build"
        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
      }
    }
  }
}
