Note on converting to new Unity Input System:

If you want to switch to using Unity's new Input System, first install the package from the Package Manager ("Input System" 
by Unity Technologies). It should prompt you to restart your Unity project. 

Once restarted, open the "2. ProTips Demo" scene and select the EventSystem gameobject in the scene hierarchy window. On 
the Standalone Input Module, you will notice it is complaining about using the Standalone Input Module. Click the button to 
replace this module with the InputSystemUIInputModule. Also remove or disable the Touch Input Module on the same EventSystem
gameobject. 

http://www.modelshark.com/content/images/ProTips-NewInputSystem.png

Now you should be able to run the demo scene and the tooltips should work fine using the new Input System. Note that you will
need to do this process on any of the demo scenes you want to convert over to the new Input System.