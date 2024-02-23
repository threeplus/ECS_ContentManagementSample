# Unity ECS Content Management Sample

In this sample, you can download all content and load any Entity Prefab or Entity Scene.

ECS Content Management System manual:
https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/content-management-delivery.html

## Quick Started
1. Import project with Unity 2022.3 or up
2. Build remote content to delivery: **Tools > Build Content**
3. Copy the remote content to your favourite HTTP server, for example, nginx
4. Open **SampleScene.scene**, find the GameObject **UIManager**, change the url to your download server
5. Build the application to test. It works in the editor. However it is better to test in the production build, Unity do a lot of tricks that only works in editor.

## Sample Functions
![image](https://github.com/threeplus/ECS_ContentManagementSample/assets/5707039/6e04f3be-b8bc-4b26-9437-3fd76b4d33ba)

Your will see the above screen in the sample.
Here is the detail:
1. **Init with all**
   
   Initialize and download all contents from the server. And you can load them using the button on the right.
2. **Init empty**
   
   Bugged or ambiguous behaviour, will by explained below.

3. **Delete content cache**
   
   Delete all content cache

4. **Load Cube Prefab** , **Load Capsule Prefab**, **Load Cylinder Scene**, **Load Capsule Scene**
   
   Load entity prefabs or scenes. Do it after init.

## Ambiguous Behaviour
When initializing the content system, RuntimeContentSystem.LoadContentCatalog is called with a **initialContentSet**. With the **initialContentSet**, contents of the ContentSet is downloaded when the content system is initializing. I cannot find other method to download contents after initialization, unless we do some hack to the internal system. Everything works fine, when we use **all** as the initialContentSet. That means downloading **all** assets when the system is initializing.
However, in a proper use case of a content distribution system, like Addressable and Asset Bundles, we are not going to download all assets at the beginning. Instead, we want to download the assets on demand, when it is needed only. We can do that in Addressable and Asset Bundles.

In Content Management System, we can do it with a broken way. We have to restart the application for 3 or more times. Keep rebooting the application, initialize content system with empty content set, and load the desired prefab or scene, reboot again if the asset is not loaded.

Here is the detail explained:
1. Launch the application, and initialize content system with empty content set. Content catalog is downloaded. Load the desired prefab or scene, it is downloaded. But the system cannot actually load the asset and stucked.
2. Reboot the application, and initialize content system with empty content set. Some dependency is downloaded. Load the desired prefab or scene again, its dependencies is downloaded, but the system is still stucked and failed to load.
3. Reboot the application, and initialize content system with empty content set. Load the desired prefab or scene again, if all dependencies is downloaded, it can be loaded successfully. If not, go to Step 2, to download any dependencies of dependencies.
