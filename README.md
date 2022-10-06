# azfile-keyless-mount  
<br />

* Azure File-Keyless-Mount or AKM in short maps Azure File share network drive onto any Windows machine without keeping Azure Storage Key on machine.  

* It works by accessing Azure Key Vault using VM's Managed Identity to retrieve Storage Key as secret.  

* AKM spins off a background worker to constantly monitor the mapped network drive, if drive is found missing due to VM restarts or transient network fault,
drive is automatically remapped. This check happens every 3 seconds.  

AKM .exe and config.yaml can be found in [./deploy](https://github.com/weixian-zhang/azfile-keyless-mount/tree/main/deploy) folder.  
.exe file is bundled as self-contained and does not require DotNet framework to be installed.  

AKM console window can be hidden by a setting in config.yaml or it can be [added as Windows Startup program](https://shellgeek.com/startup-folder-path-in-windows-server/) to cover VM restart scenario.
