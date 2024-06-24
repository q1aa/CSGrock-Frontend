
## CSGrok (.NET C#) // Frontend

Check out the [project website](https://tunnels.julin.dev/)

This project provides a lightweight HTTP tunneling tool similar to ngrok and cloudflaredtunnels, written in C#. It allows you to expose local development servers to the internet for testing and sharing purposes.

This project is designed more for demonstration purposes than for practical application.

Check out the [backend](https://github.com/q1aa/CSGrock-Backend) in order to use it!
## Installation

I recommend to download VisualStudio and open the whole project in it.

The project requires dotnet 6.0 to run, please make sure you have it installed!

git clone https://github.com/q1aa/CSGrock-Frontend.git

If you have installed VisualStudio just open the .sln file with it.

#
**When selfhosting the backend only!**

Inside the file "CSGrockLogic\Utils\StorageUtil.cs" change the "BackendURL" to "localhost:7006"
#

You can start it now (F5 key as shortcut), afterwards enter the connectiontype (allways http for now) and the port you want to forward. After clicking the "Enter" key, it will try to connect to the backend 
## Tech Stack

**Client:** C# (dotnet)

**Server:**  C# (dotnet), ASP.net, js, html, css


## FAQ

#### 'csgrok' is not recognized as an internal or external command, operable program or batch file.

This indicates, that the programm was not added to the  system environment variables successfully. Eighter could be the case, that someting went wron with the installer, or if you have it installed manually you did it wrong.
In order to add it into the  system environment variables, feel free to checkout [this artical](https://docs.delinea.com/online-help/secret-server-11-5-x/secret-launchers/launcher-configuration-and-support/adding-a-program-folder-to-the-windows-path/index.htm)
## Authors

- [@q1aa](https://github.com/q1aa)
