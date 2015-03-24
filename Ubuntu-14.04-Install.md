Installing MiNET on Ubuntu 14.04 LTS X64
# installing mono-project for building
sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get update
sudo apt-get install mono-complete mono-devel referenceassemblies-pcl
# cloning sources from repository !!!You need import ssh key from .ssh/ folder in your home to github account!!!
git clone --recursive git@github.com:NiclasOlofsson/MiNET.git

# installing all needed certificates for external components Microsoft.NET
sudo mozroots --import --machine --sync
sudo certmgr -ssl -m https://go.microsoft.com
sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo certmgr -ssl -m https://nuget.org

# building project!
cd MiNET; xbuild src/MiNET/MiNET.sln

# running Server!
cd src/MiNET/MiNET.Service/bin/Debug; mono MiNET.Service.exe
