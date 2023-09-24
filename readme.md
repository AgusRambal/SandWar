# CorgiFX

CorgiFX is a collection of shaders primarily designed for Virtual Photography, utilizing the ReShade FX language.

# CanvasFog

CanvasFog enables you to apply various types of gradients to the game scene, interacting with the depth buffer. This allows you to manually add gradients to the sky, create contrast between subjects and the background, produce silhouettes, and more.

I have reused code from similar shaders (with proper credits given) like AdaptiveFog, Emphasize, and DepthSlicer. If you are familiar with these shaders, you should have no trouble adjusting to the parameters of CanvasFog.
 
## Features
 
- **Gradients**: Choose from a variety of gradients (Linear, Radial, Strip, Diamond), each with its own customizable settings.
- **Colors with alpha channel**: In addition to selecting two colors for the gradient, you can also adjust the alpha channel to create color spots.
- **HSV gradients**: Utilize HSV gradients for smoother color transitions.
- **Color pickers**: Capture colors from pixels on the screen and incorporate them into the gradient.
- **Fog Types**: The fog type refers to the algorithm used to display the gradients. Currently, there are three types available: Adaptive Fog, Emphasize, and Depth Slicer. The first two function similarly to Frans' shaders of the same name, while the latter utilizes Prod80's shaders for depth control. In my experience, Depth Slicer is the easiest one to use.
- **Fog rotation (WIP)**: This feature allows you to rotate the fog "wall" within the game world, creating more interestic interactions with the games' depth buffer. Currently, it is only available in AdaptiveFog.
- **Blending modes**: Apply various blending modes (Multiply, Screen, Overlay, etc.) to blend the colors of the gradient with the scene.
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476225-9eedbc80-4370-11ea-8a58-57447dadf76e.png">
<i>Simple adaptive-type fog with linear gradient and normal blending</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476249-a745f780-4370-11ea-8b9a-72c1f0d28f88.png">
<i>Adaptive-type fog with linear gradient using color mode</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476250-a7de8e00-4370-11ea-8d36-cb49df021fba.png">
<i>Adaptive-type fog with linear gradient using a color with zero alpha</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476254-a7de8e00-4370-11ea-9af2-2b1df0a66b2c.png">
<i>Radial gradient with high gradient sharpness</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476256-a8772480-4370-11ea-8579-7dd8df7e8755.png">
<i>Strip type gradient with one of the colors with 0 alpha value</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476258-a8772480-4370-11ea-8184-d2b90a51fcfe.png">
<i>Radial gradient with screen blending mode</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476261-a8772480-4370-11ea-85e7-735c53225421.png">
<i>You can alter the x and y values separately on the radial gradient as well as rotating it</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/73476265-a9a85180-4370-11ea-924d-98513728f30c.png">
<i>Emphasize-type fog with lineal gradient and color blending mode</i></p>
 
<p align="center"><img src="https://user-images.githubusercontent.com/24371572/74173946-d0d50d80-4c11-11ea-8d39-b7df1f82a613.png">
<i>Adaptive-type fog with diamond gradient and CanvasMask</i></p>