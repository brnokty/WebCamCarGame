# Web Cam Car Game

## Description

In this game, you race your car around a track in a time-based competition, completing 3 laps while aiming to finish the course in the shortest possible time. During the race, banners displaying photos taken from the player's webcam are shown. At the end of the game, the scores are recorded, and all the photos taken during the game can be viewed.

## Table of Contents

- [Installation](notion://www.notion.so/GitHub-ta-Readme-Nas-l-Yaz-l-r-9f11801437c244d782c8c591efe77d43#Installation)
- [Usage](notion://www.notion.so/GitHub-ta-Readme-Nas-l-Yaz-l-r-9f11801437c244d782c8c591efe77d43#Usage)

## Installation

You need Unity 2022.2.13f1 or newer to use this project. After downloading the project files, you can open this project in Unity Hub.

## Usage

To start the game, open the "MainScene" scene in the "Scene" folder in Unity Editor and click the "Play" button. While playing the game,
In the game, you can control the vehicle using the 'WASD' keys and brake with the 'Space' key. If you find yourself stuck, you can reset the vehicle by pressing 'R'. Additionally, the game supports gamepads. If you prefer using a gamepad, the button layout is shown in the image.

![GamePad](https://github.com/user-attachments/assets/db70ba8a-172a-4dad-9564-740a8f374b34)

## Reset Car Mechanics
Of course I took precautions in case the car turns upside down or various problems in the game, but just in case, I put reset points where we can return to in case of any problem in the game. When we press the "R" key, we re-spawn at the point we are closest to. In case the game cannot solve anything, you can continue the game with the R key.

Some Gifs from gameplay;

<img src="https://github.com/user-attachments/assets/c4e25219-5759-428e-985d-005ed2339b28" width="400" height="225">
<img src="https://github.com/user-attachments/assets/c2ba645e-18ce-43aa-b2f4-a3cedcb41000" width="400" height="225">
<img src="https://github.com/user-attachments/assets/4d232b80-3d86-4c22-9891-fd6bd468ccd7" width="400" height="225">


![Gif_1](https://github.com/user-attachments/assets/c4e25219-5759-428e-985d-005ed2339b28)
![Gif_2 (2)](https://github.com/user-attachments/assets/c2ba645e-18ce-43aa-b2f4-a3cedcb41000)
![Gif_3](https://github.com/user-attachments/assets/4d232b80-3d86-4c22-9891-fd6bd468ccd7)

You can understand the game more easily by looking at the gif.

## About the Project
In order to work regularly on the project, I created various managers and thus divided the workload.

![image](https://github.com/user-attachments/assets/241667f4-7506-4045-991c-deb1b21a4a3b)

Since everyone's job is clear, I created a sustainable and easy-to-control structure.

## Car Movement

![image](https://github.com/user-attachments/assets/e7a8004c-58ca-43ee-bad8-50daa4d5f81d)

I made the car's movement mechanics with Wheel Colliders in Unity and played with the object's center of gravity to make it similar to the car physics so that the car can hold on to the ground more.

## StreamingAssets

![image](https://github.com/user-attachments/assets/3d852a27-b105-49e6-b77d-f09217bec422)

The saved photos are saved in a folder named after the user.

![image](https://github.com/user-attachments/assets/e4ad8ad1-47e1-4629-a484-7b97efdab323)

We keep the ScoreBoard in the scores.json file.

## Scene Swicther

![image](https://github.com/user-attachments/assets/f57f2c3d-db47-4303-9286-39ab0b129e7c)

When we select Scene Switcher from here

![image](https://github.com/user-attachments/assets/3e5ab105-ac1a-4e6c-946a-59ac0dff0248)

This panel opens and makes it easier to switch between scenes while developing the game.

## Folder Structure

![image](https://github.com/user-attachments/assets/693902f9-c924-4214-87da-f602f049cebf)

To avoid clutter in the game folder, I created a folder called [Game] and collected my game-related files under that folder.

![image](https://github.com/user-attachments/assets/3bef9fd4-37b7-4d7e-a1d0-91bf9b88fab8)

This way, I can reach what I'm looking for more easily.

## Banner Structure

When the game starts, we take a photo and first we show it on all banners so that the banners are not empty, then we take a new photo every 5 seconds and give it to the banners. When there are 5 photos in total for each banner, we stop taking photos or if we have moved on to the 2nd round, we stop.

![image](https://github.com/user-attachments/assets/1c5543c9-7ca1-4fde-940b-3a775d4b05be)

Banner design I add a ribbon sprite at the top and show the Player name in the banner so that the banner does not remain empty.

## Setting Panel

![image](https://github.com/user-attachments/assets/5d50a283-67b9-4277-8dc5-621866058813)

In the settings panel, normally there was only a toggle for turning the sound on and off, but since it selected the wrong camera by default on my computer, I wanted to add a camera selection setting, and since some people may be used to the mirror effect on their phones, I added a mirror setting as well.

## Player Name Panel

![image](https://github.com/user-attachments/assets/e7bb11e8-fca1-4578-9b56-385579c4c371)

When we enter our player name in the player name panel and play, we enter the game, but in order to avoid problems with the scoreboard, I prevented us from using the same username.

![image](https://github.com/user-attachments/assets/947a83ec-3ada-4e3b-ab28-f6b9c9b9d858)

When the same username is entered, a warning appears and we prevent this.
