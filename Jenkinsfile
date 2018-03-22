
def config = "Debug"
def base_folder = "AdminApp/"
def solution_file = "AdminSoft.sln"
def test_project_name = "AdminSoft.Test"
node ("master") {  
	//try{
		def test_dll_path = "${base_folder}${test_project_name}/bin/${config}/${test_project_name}.dll" 
		stage('Information') { 
				echo "env.BRANCH_NAME: ${env.BRANCH_NAME}"
		}
		stage('checkout'){
			checkout scm
			//git branch: env.BRANCH_NAME,  credentialsId: '5e3f0a7c-1045-40e9-b310-d481d65de1bf', url: 'git@github.com:ronymaychan/adminsoft.git'
		}
		stage("Restore Nuget"){
			//bat "NuGet.exe restore ${base_folder}${solution_file}"
		}
		stage("Build"){
			//bat "MSBuild.exe ${base_folder}${solution_file} /p:\"Configuration=${config}\" /p:Platform=\"Any CPU\""
		}
		stage("Testing"){
			//bat "nunit3-console.exe ${test_dll_path}"
			//nunit testResultsPattern: '*.xml'
		}
		/*if(env.BRANCH_NAME == "develop"){
			stage("Deploying develop") {
				//def publishProfile = "develop"
				//bat "MSBuild.exe ${base_folder}${solution_file}  /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=${publishProfile}"
			}
		}
		if(env.BRANCH_NAME == "master"){
			stage("Deploying tests") {
				//def publishProfile = "master"
				//bat "MSBuild.exe ${base_folder}${solution_file}  /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=${publishProfile}"
			}			
		}*/
	/*}catch(err){
		if((env.BRANCH_NAME  == "master" || env.BRANCH_NAME  == "develop") &&
			env.adminsoft_email_list != null && env.adminsoft_email_list != ""){
			emailext ( 
                subject: "FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'", 
                body: """<p>FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
					<p>BRANCH_NAME: '${env.BRANCH_NAME}</p>
                    <p>${err}</p>
                    <p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
                to: adminsoft_email_list
            )
		}
		bat "exit 1"  
    }*/
}




/*Deploy angular app*/
			/*stage("Deploying angular"){
				dir('Adminweb') {
					bat "npm install"
					bat "ng build"
				}
			}*/


/*
if( (env.BRANCH_NAME  == "master" || env.BRANCH_NAME  == "develop") &&
			env.adminsoft_email_list != null && env.adminsoft_email_list != ""){
			emailext ( 	
				subject: "SUCCESS: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'", 
				body: """<p>SUCCESS: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
					<p>BRANCH_NAME: '${env.BRANCH_NAME}</p>
					<p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
				to: env.adminsoft_email_list
			)
		}
*/