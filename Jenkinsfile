
def config = "Debug"
def base_folder = "AdminApp/"
def solution_file = "AdminSoft.sln"
def test_project_name = "AdminSoft.Test"
node ("master") {  
ws("workspace/adminsoft/${env.BRANCH_NAME.replaceAll('/', '-')}") {
	try{
		def test_dll_path = "${base_folder}${test_project_name}/bin/${config}/${test_project_name}.dll" 
		/*stage('Information') { 
				echo "	env.BRANCH_NAME: ${env.BRANCH_NAME} \n" +
				"	env.CHANGE_ID: ${env.CHANGE_ID} \n" +
				"	env.CHANGE_URL: ${env.CHANGE_URL} \n" +
				"	env.CHANGE_TITLE: ${env.CHANGE_TITLE} \n" +
				"	env.CHANGE_AUTHOR: ${env.CHANGE_AUTHOR} \n" +
				"	env.CHANGE_AUTHOR_DISPLAY_NAME: ${env.CHANGE_AUTHOR_DISPLAY_NAME} \n" +
				"	env.CHANGE_AUTHOR_EMAIL: ${env.CHANGE_AUTHOR_EMAIL} \n" +
				"	env.CHANGE_TARGET: ${env.CHANGE_TARGET} \n" +
				"	env.BUILD_NUMBER: ${env.BUILD_NUMBER} \n" +
				"	env.BUILD_ID: ${env.BUILD_ID} \n" +
				"	env.BUILD_DISPLAY_NAME: ${env.BUILD_DISPLAY_NAME} \n" +
				"	env.JOB_NAME: ${env.JOB_NAME} \n" +
				"	env.JOB_BASE_NAME: ${env.JOB_BASE_NAME} \n" +
				"	env.BUILD_TAG: ${env.BUILD_TAG}" +
				"	env.EXECUTOR_NUMBER: ${env.EXECUTOR_NUMBER} \n" +
				"	env.NODE_NAME: ${env.NODE_NAME} \n" +
				"	env.NODE_LABELS: ${env.NODE_LABELS} \n" +
				"	env.JENKINS_HOME: ${env.JENKINS_HOME} \n" +
				"	env.JENKINS_URL: ${env.JENKINS_URL} \n" +
				"	env.BUILD_URL: ${env.BUILD_URL} \n" +
				"	env.JOB_URL: ${env.JOB_URL} \n"
		}*/
		stage('checkout'){
			checkout scm
		}
		stage("Restore Nuget"){
			bat "NuGet.exe restore ${base_folder}${solution_file}"
		}
		stage("Build"){
			bat "MSBuild.exe ${base_folder}${solution_file} /p:\"Configuration=${config}\" /p:Platform=\"Any CPU\""
		}
		stage("Testing"){
			bat "nunit3-console.exe ${test_dll_path}"
			nunit testResultsPattern: '*.xml'
		}
		if(env.BRANCH_NAME == "develop"){
			stage("Deploying develop") {
				def publishProfile = "develop"
				bat "MSBuild.exe ${base_folder}${solution_file}  /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=${publishProfile}"
			}
		}
		if(env.BRANCH_NAME == "master"){
			stage("Deploying tests") {
				def publishProfile = "master"
				bat "MSBuild.exe ${base_folder}${solution_file}  /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=${publishProfile}"
			}			
		}
	}catch(err){
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
    }
}
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