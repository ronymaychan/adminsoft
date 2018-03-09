node ("master") {  
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
    		"	env.JOB_URL: ${env.JOB_URL} \n"
    }
    stage('Build') { 
        // 
    }
    stage('Test') { 
        // 
    }
    stage('Deploy') { 
        // 
    }
}