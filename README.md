# KeyAuth-Loader-Emulation
KeyAuth is a **brilliant** auth, however too many skids are using KeyAuth without taking sufficient measures to protect their application.
I have noticed most loaders using KeyAuth gather the user's SID ``WindowsIdentity.GetCurrent().User.Value`` for the HWID (anti-keysharing) algorithm. This can be spoofed/modified via multiple ways, one being to use an application that uses KeyAuth but feeding in the skid's KeyAuth app details into it and then passing through any value for HWID, PC username, etc.
