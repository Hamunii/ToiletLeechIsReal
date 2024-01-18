### Blender Basics

> [!TIP]  
> Don't press random keys, as Blender has a lot of keyboard shortcuts and you might have no idea what you just did or how to undo it. That said, keyboard shortcuts can speed up your workflow by a lot, and you can use this [Blender Shortcuts Cheat Sheet](https://docs.google.com/document/d/1zPBgZAdftWa6WVa7UIFUqW_7EcqOYE0X743RqFuJL3o/edit?pli=1#heading=h.ftqi9ub1gec3) by Blender Guru, which can be useful.

If you have absolutely no experience with Blender, the 4 first parts of this series will be relevant.  
[Blender 4.0 Beginner Donut Tutorial](https://www.youtube.com/playlist?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) (playlist) - Blender Guru
- [Part 1: Introduction](https://youtu.be/B0J27sf9N1Y?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Introduces the very basics of Blender
- [Part 2: Basic Modeling](https://youtu.be/tBpnKTAc5Eo?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Self-explanatory (Learn to model a Donut)
- [Part 3: Modeling the Icing](https://youtu.be/AqJx5TJyhes?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z&t=42) - introduces more advanced modeling techniques (Detailing)
- [Part 4: Sculpting](https://youtu.be/--GVNZnSROc?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Sculpting can be especially useful when modeling organic things

### Modeling

- [Fast Character Modeling with the Skin Modifier in Blender](https://youtu.be/DAAwy_l4jw4) - Joey Carlino
    - Introduces a super cool and easy modeling technique. I recommend this a lot for learning to make basic initial meshes for characters or creatures.

// TODO: add more resources

### Modeling - Common Issues

**My mesh looks inverted in Blender or when imported to Unity.**  
This is because your normals got inverted in one way or another. Select your mesh in Edit Mode, press A to select everything, press Shift+N to recalculate your normals (do not select "Inside", that is the flipped state).  
If this doesn't fix the problem after importing to Unity, you likely have resized your object by a negative amount. This looks normal in Blender, but not in Unity. To fix this, go into Object Mode, select your object, press Ctrl+A, select apply scale. Now your normals should have flipped in Blender. Now, recalculate normals.

// TODO: add more stuff

### Materials & Texturing, UV Unwrapping

> [!NOTE]  
> Unity does not understand Blender's shader node system. If you use it for anything other than the princibled BSDF, you will have to bake your material as a texture before it will work in Unity. Also make note of the fact that Lethal Company automatically adds its own "style" to everything, so you don't need to worry about that. However, textures are not necessary so you can basically skip this section entirely.

// TODO: add resources

### Rigging

- [Tutorial: My New Rigging Workflow in Blender](https://youtu.be/BiPoPMnU2VI) - Polyfjord
    - Inverse kinematics on a mechanical character. Very useful for rigging legs.
- [Rigging for impatient people - Blender Tutorial](https://youtu.be/DDeB4tDVCGY) - Joey Carlino
    - Includes a lot of useful information about rigging, but it's a very fast paced video. Can be fairly hard to follow for a complete beginner. Likely a better watch after you've seen the more basic introductory type tutorials first.
- [How to Rig and Animate in BLENDER!](https://youtu.be/1khSuB6sER0) - ProductionCrate
    - Learn how to make a rig for a humanoid character, fix issues with Blender's automatic weights feature, as well as inverse kinematics. 

// TODO: add more resources

### Animation & NLA (Nonlinear Animation) Editor

> [!NOTE]  
> We put our individual animations in the NLA Editor so we can use them separately in Unity. The length of the animation in Unity will be the length that you set in the NLA editor. This is important to know if you set an animation cycle to repeat a certain amount of times in Blender when you want to for example preview it in combination with your other animations.

- [The Nuts and Bolts of Blender's animation system](https://youtu.be/p3m57yAcsi0) - CGDive
    - Introduces concepts in a very in-depth way. Introduces Timeline, Dope Sheet, Graph Editor, NLA Editor, Actions.
- [Un-confusing the NLA Editor (Nonlinear Animation)](https://youtu.be/tAo7HxxxA08) - GCDive
    - A more in-depth video about the NLA Editor. Do note though, we do not need to do anything complex with the NLA Editor.
- [Become a PRO at Animation in 25 Minutes | Blender Tutorial](https://youtu.be/_C2ClFO3FAY) - CG Geek
    - Animating a walk cycle. Uses Timeline, Dope Sheet and Graph Editor. Uses references for animation.
- [Character animation for impatient people - Blender Tutorial](https://youtu.be/GAIZkIfXXjQ) - Joey Carlino
    - If you don't want to make and rig your own models.

// TODO: add more resources

### Animation - Common Issues

**Objects in Model appear in different places as in Blender when exporting to Unity.**
This might be because you have directly animated an object, instead of an armature. Try parenting your object to an armature and remake your animation with that.

**Animations are broken in Unity.**
This might be because you have animations with the same name in your NLA editor. Make sure your animations have unique names.

### Exporting Assets For Unity

> [!IMPORTANT]
> You might or might not need to apply All Transforms of your 3D Model by hovering your cursor over 3D Viewport and doing `Ctrl+A` -> `All Transforms` for your model to appear in Unity as it does in Blender. Contributions to this guide are welcome.

To export your model, go to: `File` -> `Export` -> `FBX (.fbx)`  
This will open our FBX exporter window, where we have some options available to us. The most important thing here however is the transform section. Because of the differences in Blender's and Unity's coordinate systems, exporting your model is not quite as straight-forward as you'd think. It's very easy to get your model pointing in the wrong direction, being sideways, or even upside down if you don't have correct values set.

Even I don't understand how any it really works, but our model points in the negative Z direction, and in the transfrom section, we have set `Forward` to `-Z Forward`, and `Up` to `Y Up` and our model appears correctly in Unity.  
![Screenshot: Export as FBX Settings](./ForTutorial/BlenderExportAsFBX.png)

**Next: [Unity page](./Unity.md)**