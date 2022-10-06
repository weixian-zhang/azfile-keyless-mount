# azfile-keyless-mount  
<br />

* Azure File-Keyless-Mount or AKM maps Azure File share network drive onto any Windows machine without keeping Azure Storage Key on machine.  

* It works by accessing Azure Key Vault using VM's Managed Identity to retrieve Storage Key as secret.  

* AKM spins off a background worker to constantly monitor the mapped network drive, if drive is found missing due to VM restarts or transient network fault,
drive is automatically remapped. This check happens every 3 seconds.  

* Console window can be hidden to simulate running as background or as a "Window Service"

* option to send logs to Log Analytics workspace

### Deployment  

* AzFileKeylessMount.exe and config.yaml can be found in [./deploy](https://github.com/weixian-zhang/azfile-keyless-mount/tree/main/deploy).  
AzFileKeylessMount.exe file is bundled as self-contained so it does not require DotNet framework to be installed.  
Once the console window is configured to be hidden, running [AzFileKeylessMount.exe](https://github.com/weixian-zhang/azfile-keyless-mount/blob/main/deploy/AzFileKeylessMount.exe) makes AKM run like a background app

* AKM can be [added as Windows Startup program](https://shellgeek.com/startup-folder-path-in-windows-server/) to cover VM restart scenario.  
* 
* <b>Not tested</b>: when creating "new" VMs, another option is to leverage [Custom Script Extension](https://learn.microsoft.com/en-us/azure/virtual-machines/extensions/custom-script-windows) to deploy AKM
