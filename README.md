# LightweightWebServer

A simple HTTP server implemented in C# that handles HTTP GET requests over TCP/IP connections. It serves static files such as HTML, JPG, TXT, and GIF, making it ideal for local development and testing environments.

## Features:
- Handles HTTP GET requests.
- Serves static files: HTML, JPG, TXT, and GIF.
- Built using C# and TCP/IP connections.
- Supports custom directory, IP, and port via command-line arguments.

## Setup Instructions:

1. **Clone the Repository:**
   Open your terminal or command prompt and run:
   ```bash
   git clone https://github.com/jacob-atienza/LightweightWebServer.git
   ```

2. **Open the Solution:**
   Navigate to the project directory:
   ```bash
   cd LightweightWebServer
   ```
   Open the `LightweightWebServer.sln` file in Visual Studio.

3. **Build the Project:**
   In Visual Studio, build the project by selecting **Build** > **Build Solution** from the menu. This will generate the `.exe` file in the `bin/Debug/net5.0/` (or appropriate framework version) folder.

4. **Run the Server with Custom Settings:**
   After building the project, you can run the server by executing the generated `.exe` and passing the required command-line arguments. 

   The server accepts the following arguments:

   - `-webRoot=[DIRECTORY]`: Specify the directory to serve static files from (default is `wwwroot`).
   - `-webIP=[IP_ADDRESS]`: Specify the IP address the server should listen on (default is `127.0.0.1`).
   - `-webPort=[PORT]`: Specify the port the server should listen on (default is `8080`).

   ### Example Usage:
   After building the project, navigate to the output folder (e.g., `bin/Debug/net5.0/`), and run the executable like this:

   ```bash
   LightweightWebServer.exe -webRoot=C:\MyWebsite -webIP=192.168.1.100 -webPort=5000
   ```

   This will start the server with the following settings:
   - IP address: `192.168.1.100`
   - Port: `5000`
   - Directory: `C:\MyWebsite`

5. **Access the Server:**
   Once the server is running, open your web browser and navigate to:
   ```bash
   http://[IP]:[Port]
   ```
   Replace `[IP]` and `[Port]` with the values you specified. For example, `http://192.168.1.100:5000`.

   You should place your static files (HTML, JPG, TXT, GIF) in the custom directory you specified in the previous step.

## Note:
This server is intended for local development and testing purposes. It is not recommended for production environments.
