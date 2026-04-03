# Remote Desktop Application (GUI Basic) - UDM_06

## Project Description
Remote computer screen control applications allow users to connect to and control another computer over a network.
The system supports real-time screen image transmission and processes mouse and keyboard input from the user to the partner computer.
The application is designed to assist technical staff in troubleshooting customer issues, while ensuring ease of use for general users through a simple, user-friendly interface. The software is suitable for learning, technical support, and remote computer management.

## Members
1. Nguyễn Phú Quang
2. Ngô Xuân Sơn
3. Dương Lê Sơn Hào
4. Nguyễn Đăng Khoa  
5. Lã Hải Long  
6. Đỗ Văn Long

## Assignment
1. Nguyễn Phú Quang
   - Project Setup + System Integration + Testing
     +  Create repository
     +  Setup Client/Server solution
     +  Manage GitHub (pull/push, merge)
     +  Integrate modules (UI, Socket, Screen)
     +  Perform system testing
     +  Detect and report bugs 
2. Nguyễn Đăng Khoa
   - Server UI
     + Design ServerForm
     + Display status (Waiting, Connected)
     + Show IP and Port
     + Add control buttons (Start Server, etc.)
     + Organize layout
3. Dương Lê Sơn Hào and Lã Hải Long
   - Client UI
     + Design ClientForm
     + Input IP and Port
     + Button Connect
     + Display remote screen
     + Optimize layout  

4. Đỗ Văn Long & Ngô Xuân Sơn
   - Socket Programming
     + Implement ServerSocketService (listen client)
     + Implement ClientSocketService (connect server)
     + Send/receive test messages
     + Handle connection errors
     + Log connection status
## Objectives
1. Purpose of the Application
   - Develop a Remote Desktop application that allows users to connect to another computer remotely
   - Enable real-time screen viewing from the server
   - Allow users to control the remote computer using mouse and keyboard in real-time
   - Support basic remote management and monitoring
2. Development Goals
   - Design a Client-Server architecture using socket programming
   - Build user interfaces for both Server and Client using Windows Forms
   - Implement real-time network communication between client and server
   - Capture, encode, and transmit screen data from server to client
   - Handle remote input events (mouse and keyboard)
   - Integrate all modules into a complete working system
   - Perform testing and debugging to ensure system stability
3. Learning Outcome
   - Understand how real-time communication systems work
   - Gain experience in network programming and system integration
## Main Functions

1. Server Functions
   - Start the server and listen for client connections
   - Display server IP address and port number
   - Show current connection status (Waiting, Connected, Disconnected)
   - Accept connection requests from clients
   - Send screen data to the connected client
   - Receive control commands from the client
2. Client Functions
   - Enter server IP address and port number
   - Connect to the remote server
   - Display the remote screen from the server
   - Send mouse actions to the server
   - Send keyboard input to the server
   - Show connection status and error messages
3. System Functions
   - Establish communication between server and client using sockets
   - Transmit screen data in real-time
   - Support remote monitoring and control
   - Handle connection errors and disconnection events
   - Integrate all modules into one complete application
## Technologies Used
- Programming Language: C#
- Framework: .NET (Windows Forms)
- Network Communication: System.Net.Sockets
- Architecture: Client-Server
- Image Processing: System.Drawing
- Input Handling: Windows API
- IDE: Visual Studio
- Version Control: Git & GitHub
## System Architecture
The system is designed based on a Client-Server architecture.
- The Server captures screen data and sends it to the client
- The Client connects to the server and displays the remote screen
- The Client sends mouse and keyboard events to the server
- Communication is established using TCP sockets
Data Flow:
1. Client connects to Server
2. Server accepts connection
3. Server captures screen
4. Server sends screen data to Client
5. Client displays screen
6. Client sends input events
7. Server executes input actions
## 📂 Project Structure
- Server/
  + ServerForm.cs
  + ServerSocketService.cs
- Client/
  + ClientForm.cs
  + ClientSocketService.cs
- Services/
  + Network handling
  + Data processing
- UI/
  + Forms and layout




