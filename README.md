Contained in this repo are selected example scripts from my Unity Previsualization Dialogue System.

The website with the latest screenshots and videos (of an older version) can be found here:  
[https://www.ryanportfolio.net/](https://www.ryanportfolio.net/)

---

## Directory Overview ##

---

### CameraCalculator

*Used in calculating the camera composition in the preview window and during runtime.*

- CameraCalculator.cs  
- CameraSettings.cs  

---

### DialoguePlayerStateController

*The state machine and controller that traverses the branching node network based on player choices and input.*

- NodeStateController.cs  

---

### DialoguePreview

*Previews the character and camera composition.*

- DialoguePreview.cs  
- PreviewRenderer.cs  

---

### NodeDrawers

*Essentially a wrapper or the view to the Node to display the relevant information.*

- DecisionNodeDrawer.cs  
- DialogueNodeDrawer.cs  
- NodeDrawerBase.cs  
- NodeDrawerFactory  

---

### Serialization

*Saving and deserializing the Node network.*

- NodeDeserializer.cs  
- SaveFile.cs  

---
