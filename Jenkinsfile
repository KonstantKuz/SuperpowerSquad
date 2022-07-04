pipeline {
    agent none
    options {
        timestamps()
        timeout(time: 60, unit: 'MINUTES')
    }
    parameters {
        choice(choices: ['Android', 'iOS'], name: 'Platform') 
        booleanParam(name: 'NotifyTesters', defaultValue: true, description: 'Notify testers in AppCenter about this build (ignored if not published to AppCenter)')
        booleanParam(name: 'Publish', defaultValue: false, description: 'Publish this build to AppCenter or TestFlight')
        booleanParam(name: 'BuildApk', defaultValue: true, description: 'Build apk, only for android')        
        booleanParam(name: 'BuildAab', defaultValue: false, description: 'Build aab, only for android')
        booleanParam(name: 'IpaForAppStore', defaultValue: false, description: 'Build ipa for publishing in AppStore, only for iOS')
        booleanParam(name: 'Clean', defaultValue: false, description: 'Delete and reimport assets')
        booleanParam(name: 'DebugConsole', defaultValue: true, description: 'Enable debug console, only for apk')
        choice(choices: ['DEBUG', 'TRACE', 'INFO', 'WARN', 'ERROR'], name: 'LoggerLevel', description: 'Logger Level') 
    }
    environment {
        OUTPUT_FILE_NAME = 'survivors'
        GIT_SSL_NO_VERIFY=true
    }      
    stages {                                    
        stage ('Android') {
            environment {
                UNITY_PATH = '/opt/unity/Editor/Unity'
            }        
            when {
                beforeAgent true
                expression { return params.Platform == "Android" }
            }
            agent {
                node {
                    label 'android && unity2020'
                    customWorkspace '/home/jenkins/slave-root/survivors'
                }
            }
            stages {
                stage ('Clear') {
                    when {
                        expression { return params.Clean }
                    } 
                    steps {                                  
                        sh 'rm -rf ./Library'
                        sh 'rm -rf ./Temp'
                        sh 'rm -rf ./build'                                   
                    }
                }
                stage ('Apk') {
                    when {
                        expression { return params.BuildApk }
                    }                 
                    stages {                   
                        stage ("Build") { 
                            options {
                                lock('UnityLicense')
                            }           
                            steps {
                                withCredentials([usernamePassword(credentialsId: 'UnityUser', usernameVariable: 'UNITY_USER_NAME', passwordVariable: 'UNITY_USER_PASSWORD'), 
                                        string(credentialsId: 'UnityLicenseKey', variable: 'UNITY_LICENSE')]) {                                   
                                    sh 'xvfb-run --auto-servernum --server-args="-screen 0 640x480x24" $UNITY_PATH -batchmode -nographics -quit -serial $UNITY_LICENSE -username $UNITY_USER_NAME -password $UNITY_USER_PASSWORD -logFile -'               
                                }  
                                script {
                                    UNITY_PARAMS=''
                                    if(params.DebugConsole) {
                                        UNITY_PARAMS=UNITY_PARAMS + '-debugConsole '
                                    }
                                    UNITY_PARAMS=UNITY_PARAMS + '-loggerLevel ' + params.LoggerLevel  
                                }   
   
                                withCredentials([string(credentialsId: 'SurvivorsAndroidKeystorePass', variable: 'KEYSTORE_PASS'), 
                                        gitUsernamePassword(credentialsId: 'gitlab_inspiritum_smash_master', gitToolName: 'Default')]) {
                                    sh '$UNITY_PATH -nographics -buildTarget Android -quit -batchmode -projectPath . -executeMethod Editor.Builder.BuildAndroid ' + UNITY_PARAMS + '-keyStorePassword $KEYSTORE_PASS -noUnityLogo -outputFileName $OUTPUT_FILE_NAME -logFile -'              
                                }                                                                                  
                            }   
                            post {
                                always {
                                    sh script: '$UNITY_PATH -batchmode -nographics -returnlicense -logFile -', label: "ReturnLicense"
                                }
                            }     
                        }                 
                        stage ('Store') {
                            steps {
                                archiveArtifacts artifacts: "build/${OUTPUT_FILE_NAME}.apk"              
                            }
                        }                                               
                    }
                }
                stage ('Aab') {
                    when {
                        expression { return params.BuildAab}
                    }
                    stages {                        
                        stage ("Build") {    
                            options {
                                lock('UnityLicense')
                            }                                              
                            steps {
                                sh "rm -f build/*.symbols.zip"
                                withCredentials([usernamePassword(credentialsId: 'UnityUser', usernameVariable: 'UNITY_USER_NAME', passwordVariable: 'UNITY_USER_PASSWORD'), 
                                        string(credentialsId: 'UnityLicenseKey', variable: 'UNITY_LICENSE')]) {                                   
                                    sh 'xvfb-run --auto-servernum --server-args="-screen 0 640x480x24" $UNITY_PATH -batchmode -nographics -quit -serial $UNITY_LICENSE -username $UNITY_USER_NAME -password $UNITY_USER_PASSWORD -logFile -'               
                                }    
                                script {
                                    UNITY_PARAMS=''
                                    UNITY_PARAMS=UNITY_PARAMS + '-loggerLevel ' + params.LoggerLevel
                                }                                                                                                                                                             
                                withCredentials([string(credentialsId: 'SurvivorsAndroidKeystorePass', variable: 'KEYSTORE_PASS'), 
                                        gitUsernamePassword(credentialsId: 'gitlab_inspiritum_smash_master', gitToolName: 'Default')]) {
                                    sh '$UNITY_PATH -nographics -buildTarget Android -quit -batchmode -projectPath . -executeMethod Editor.Builder.BuildAndroid ' + UNITY_PARAMS + '-buildAab -noUnityLogo -keyStorePassword $KEYSTORE_PASS -outputFileName $OUTPUT_FILE_NAME -logFile -'              
                                }
                            }
                            post {
                                always {
                                    sh script: '$UNITY_PATH -batchmode -nographics -returnlicense -logFile -', label: "ReturnLicense"
                                }
                            }                             
                        }
                        stage ('Store') {
                            steps {
                                archiveArtifacts artifacts: "build/${OUTPUT_FILE_NAME}.aab,build/*.symbols.zip"            
                            }
                        }                        
                    }
                }   
            }                                          
        }
        stage ('iOS') {
            environment {
                UNITY_PATH = '/Applications/Unity/Hub/Editor/2020.3.17f1/Unity.app/Contents/MacOS/Unity'
                LANG='en_US.UTF-8'
                KEYCHAIN_PATH='/Users/jenkins/Library/Keychains/jenkins.keychain-db'
                DEVELOPMENT_PROFILE_ID='81b6c60f-221b-4a49-985b-1ccbf66305a9'
                DISTRIBUTION_PROFILE_ID='f3bf7e3c-649d-43a9-a118-cb3519a6452c'
                DEVELOPMENT_CODE_SIGN_IDENTITY='Apple Development: Ludd Ludd (NR2QZ5TJDW)'
                DISTRIBUTION_CODE_SIGN_IDENTITY='Apple Distribution: Feofun Limited (8Y9KH6XT49)'
                IPA_NAME='survivors'  //Alas, it is not taken from OUTPUT_FILE_NAME. But from module name in project...
                IPA_FULL_PATH="build/xcode/build/Release-iphoneos/build/${IPA_NAME}.ipa"
            }         
            when {
                beforeAgent true
                expression { return params.Platform == "iOS" }
            }
            agent {
                node {
                    label 'iOS && unity'
                    customWorkspace '/Users/jenkins/slave/survivors'
                }
            }
            stages {
                stage ('Clear') {
                    when {
                        expression { return params.Clean }
                    } 
                    steps {                                  
                        sh 'rm -rf ./Library'
                        sh 'rm -rf ./Temp'
                        sh 'rm -rf ./build'                                   
                    }
                }
                stage ('Unity') {
                    options {
                        lock('UnityLicense')
                    }                  
                    steps {                    
                        withCredentials([usernamePassword(credentialsId: 'UnityUser', usernameVariable: 'UNITY_USER_NAME', passwordVariable: 'UNITY_USER_PASSWORD'), 
                                string(credentialsId: 'UnityLicenseKey', variable: 'UNITY_LICENSE'), 
                                gitUsernamePassword(credentialsId: 'gitlab_inspiritum_smash_master', gitToolName: 'Default')]) {                                  
                            sh '$UNITY_PATH -batchmode -nographics -quit -serial $UNITY_LICENSE -username $UNITY_USER_NAME -password $UNITY_USER_PASSWORD -projectPath . -logFile -'               
                        }    
   
                        script {
                            UNITY_PARAMS=''
                            if(params.IpaForAppStore) {
                                UNITY_PARAMS=UNITY_PARAMS + '-distribution -provisionProfileId ' + DISTRIBUTION_PROFILE_ID
                            } else {
                                UNITY_PARAMS=UNITY_PARAMS + '-provisionProfileId ' + DEVELOPMENT_PROFILE_ID                
                            }
                            if(params.DebugConsole) {
                                UNITY_PARAMS=UNITY_PARAMS + '-debugConsole '
                            }
                            UNITY_PARAMS=UNITY_PARAMS + '-loggerLevel ' + params.LoggerLevel
                        }         
                           
                        withCredentials([gitUsernamePassword(credentialsId: 'gitlab_inspiritum_smash_master', gitToolName: 'Default')]) {
                            sh '$UNITY_PATH -nographics -buildTarget iOS -quit -batchmode -projectPath . -executeMethod Editor.Builder.BuildIos ' + UNITY_PARAMS + ' -noUnityLogo -logFile -'                        
                        }
                    }
                    post {
                        always {
                            withCredentials([gitUsernamePassword(credentialsId: 'gitlab_inspiritum_smash_master', gitToolName: 'Default')]) {
                                sh script: '$UNITY_PATH -batchmode -nographics -returnlicense -projectPath . -logFile -', label: "ReturnLicense"
                            }
                        }
                    }                     
                } 
                stage ('XCode') {
                    steps {
                        script {
                            if(params.IpaForAppStore) {
                                IPA_EXPORT_METHOD='app-store'
                                PROVISION_PROFILE_UUID=DISTRIBUTION_PROFILE_ID
                                CODE_SIGN_IDENTITY=DISTRIBUTION_CODE_SIGN_IDENTITY                              
                            } else {
                                IPA_EXPORT_METHOD='development'
                                PROVISION_PROFILE_UUID=DEVELOPMENT_PROFILE_ID
                                CODE_SIGN_IDENTITY=DEVELOPMENT_CODE_SIGN_IDENTITY                                
                            }
                        }
                    
                        withCredentials([string(credentialsId: 'jenkins_mac_keychain_pass', variable: 'KEYCHAIN_PASSWORD')]) {
                            sh 'security -v unlock-keychain -p $KEYCHAIN_PASSWORD $KEYCHAIN_PATH'
                        }
                        sh 'cat ./podfile_patch.txt >> ./build/xcode/Podfile'
                        sh '/usr/local/bin/pod install --repo-update --project-directory=./build/xcode'                        
                        withCredentials([string(credentialsId: 'jenkins_mac_keychain_pass', variable: 'KEYCHAIN_PASSWORD')]) {                      
                            xcodeBuild buildIpa: true, 
                                cleanBeforeBuild: false, 
                                cleanResultBundlePath: false, 
                                configuration: 'Release', 
                                developmentTeamID: '8Y9KH6XT49', 
                                ipaExportMethod: IPA_EXPORT_METHOD, //development, app-store, ad-hoc
                                ipaName: IPA_NAME, //seems to be ignored
                                ipaOutputDirectory: './build', 
                                provisioningProfiles: [[provisioningProfileAppId: 'com.feofunlimited.survivors', provisioningProfileUUID: PROVISION_PROFILE_UUID]], 
                                signingMethod: 'manual', 
                                xcodeProjectPath: './build/xcode', 
                                xcodeSchema: 'Unity-iPhone',
                                xcodebuildArguments: 'PROVISIONING_PROFILE="' + PROVISION_PROFILE_UUID + '" CODE_SIGN_IDENTITY="' + CODE_SIGN_IDENTITY + '"',
                                copyProvisioningProfile: false,
                                keychainPath: 'jenkins.keychain-db',
                                keychainPwd: hudson.util.Secret.fromString(KEYCHAIN_PASSWORD),
                                manualSigning: true,
                                unlockKeychain: true,
                                compileBitcode: false,
                                uploadBitcode: false,
                                uploadSymbols: false    
                        }                  
                    }
                }
                stage ('Store') {
                    steps {
                        archiveArtifacts artifacts: "${IPA_FULL_PATH},build/xcode/build/Release-iphoneos/build/${OUTPUT_FILE_NAME}-dSYM.zip"              
                    }
                }  
                stage('Publish') {
                    when {
                        expression { return params.Publish && params.IpaForAppStore}
                    }              
                    steps {
                        withCredentials([usernamePassword(credentialsId: 'AppStoreUser', usernameVariable: 'APPSTORE_USER_NAME', passwordVariable: 'APPSTORE_USER_PASSWORD')]) {
                            sh 'xcrun altool --upload-app -f ${IPA_FULL_PATH} -t ios -u $APPSTORE_USER_NAME -p $APPSTORE_USER_PASSWORD'
                        }
                    }
                }                 
            }             
        }        
    }
}               