def config              = "Debug"
def base_folder         = "AdminApp/"
def solution_file       = "AdminSoft.sln"
def test_project_name   = "AdminSoft.Test"
def developers_email	= ""


node ("master") {  
	try{

		def test_dll_path       =   "${base_folder}${test_project_name}/bin/${config}/${test_project_name}.dll" 

		stage('Information') { 
			if(isUnix()){
					echo 'SO UNIX'
				}else{
					echo 'SO WINDOWS'
				}
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
				"	env.JOB_URL: ${env.JOB_URL} \n" +
				"	developers_email: ${developers_email} \n"
		}
		stage('checkout'){
			git branch: env.BRANCH_NAME,  credentialsId: '5e3f0a7c-1045-40e9-b310-d481d65de1bf', url: 'git@github.com:ronymaychan/adminsoft.git'
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

		if(env.BRANCH_NAME == "master"){
			stage("Deploying tests") {
				def publishProfile = "testing"
				bat "MSBuild.exe ${base_folder}${solution_file} /p:\"Configuration=${config}\" /p:Platform=\"Any CPU\" /p:DeployOnBuild=true /p:PublishProfile=${publishProfile}"
			}
		}
		if(developers_email != null){
			emailext ( 	
				subject: "SUCCESS: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'", 
				body: """<p>SUCCESS: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
					<p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
				to: developers_email
			)
		}
	}catch(err){
		if(env.developers_email != null){
			emailext ( 
                subject: "FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'", 
                body: """<p>FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
                    <p>${err}</p>
                    <p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
                to: developers_email
            )
		}
		bat "exit 1"  
    }
}