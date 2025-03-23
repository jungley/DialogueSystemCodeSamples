Contained in this repo are selected example scripts from my Unity Previsualization Dialogue System.

The website with the latest screenshots and videos (of an older version) can be found here:  
[https://www.ryanportfolio.net/](https://www.ryanportfolio.net/)

---

## Directory Overview ##

---

### CameraCalculator

*Used in calculating the camera composition in the preview window and during runtime.*

- [CameraCalculator.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/CameraCalculator/CameraCalculator.cs)  
- [CameraSettings.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/CameraCalculator/CameraSettings.cs)  

---

### DialoguePlayerStateController

*The state machine and controller that traverses the branching node network based on player choices and input.*

- [NodeStateController.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/DialoguePlayerStateController/NodeStateController.cs)  

---

### DialoguePreview

*Previews the character and camera composition.*

- [DialoguePreview.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/DialoguePreview/DialoguePreview.cs) 
- [PreviewRenderer.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/DialoguePreview/PreviewRenderer.cs)  

---

### NodeDrawers

*Essentially a wrapper or the view to the Node to display the relevant information.*

- [DecisionNodeDrawer.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/NodeDrawers/DecisionNodeDrawer.cs)
- [DialogueNodeDrawer.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/NodeDrawers/DialogueNodeDrawer.cs)  
- [NodeDrawerBase.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/NodeDrawers/NodeDrawerBase.cs)  
- [NodeDrawerFactory](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/NodeDrawers/NodeDrawerFactory.cs)  

---

### Nodes
- [DecisionNode.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Nodes/DecisionNode.cs)
- [DialogueNode.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Nodes/DialogueNode.cs)
- [StartNode.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Nodes/StartNode.cs)
- [Node.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Nodes/Node.cs)

### Serialization

*Saving and deserializing the Node network.*

- [NodeDeserializer.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Serialization/NodeDeserializer.cs)
- [SaveFile.cs](https://github.com/jungley/DialogueSystemCodeSamples/blob/main/Serialization/SaveFile.cs)  

---
