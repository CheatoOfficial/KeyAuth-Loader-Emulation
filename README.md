# KeyAuth Loader Emulation
**Showcase: https://www.youtube.com/watch?v=RfjuwVc8-qE&t=434s**

KeyAuth is a **brilliant** auth and I would recommend it for beginner-to-intermediate programmers, however too many skids are using KeyAuth without taking sufficient measures to protect their application.

**(THIS IS NOT THE SAME EMULATION AS KEYAUTH "BYPASSES/EMULATORS". THE TERM EMULATION HERE REFERS TO "EMULATING" SOME SKID LOADER BY USING THE SAME KEYAUTH APPLICATION INFORMATION TO MAKE THIS TOOL LOOK LIKE IT IS THE LOADER.)**

I have noticed most loaders using KeyAuth gather the user's SID ``WindowsIdentity.GetCurrent().User.Value`` for the HWID (anti-keysharing) algorithm. This can be spoofed/modified via multiple ways, one being to use an application that uses KeyAuth but feeding in the skid's KeyAuth app details into it and then passing through any value for HWID, PC username, etc.

If you know the name of a KeyAuth variable, authenticate and put the variable name in the **Variable ID** field, then press **Get Var**.

You can do the same with KeyAuth files, authenticate and put the file ID in the **File ID** field, then press **Get File**. If this tool can get the file, it will be saved with the file ID as filename. You can bruteforce file IDs as they are six digits long (please don't bully me for my poor attempt in splitting the randomised number array of 0-999999 into eigths).

You can even get and set user variables, send log messages (with custom username), pass through custom checksum (MD5 hash) to pretend this tool "is" the skid loader (press **Get CHK** then select the loader you want to access).

You can get Session ID, number of keys, request encryption key, app version, number of users, number of online users, customer panel link and download link.

I will not demonstrate how to use this tool at this point, please do not ask me. Look at the source code and figure it out yourself - it's quite simple.

**KeyAuth client-sided ban function has not been added.**
