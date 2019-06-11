using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Xml;
using System.Reflection;
using SimpleJSON;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;

namespace PhotoUpload
{
    class FileManager : Program//esta clase es responsable de subir los archivos
    {
        public const string FolderName_WrongFiles = "wrongFiles";
        public const string FolderName_FilesWithErrors = "filesWithErrors";
        public const string FolderName_BackUp = "backup";

        public static async Task UploadFile(string filename, Service service, string fullPath)
        {
            string serverURL;                                                           //en estas lineas se manda a llamar el servidor o el servidor de test

            if (isTestServer)
                serverURL = testServerURL;
            else
                serverURL = productionServerURL;

            MultipartFormDataContent multipart = new MultipartFormDataContent();

            HttpClient client = new HttpClient();

            List<string> listDirPhotos = new List<string>();                                        //listas dde fotos
            List<string> listFilesName = new List<string>();

            //Check if file exist:
            if (File.Exists(filename))
            {
                XmlDocument xmlDoc = new XmlDocument();            //si existen archivos genera un archivo XML

                try
                {
                    xmlDoc.Load(filename);                          //carga el archivo por su nombre
                }
                catch (XmlException e)                              //de haber un error cargando el archivo como un xml
                {
                    //File is not a correct xml:
                    log.SaveToFile("ERROR trying to load XMl file. " + e);      //
                    MoveFileToFolder(filename, FolderName_WrongFiles);          //de ocurrir una excepcion en el archivo mueve estos al folder de archivos equiocados
                    return;
                }
                
                XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;      //anota loas archivos en una lista de archivos XML
                if (nodes != null && nodes.Count > 0)
                {
                    
                    foreach (XmlNode child in nodes)                                                     //para  cada coleccion de XMl
                    {
                        switch (child.Name)
                        {
                            case "files":
                                foreach (XmlNode myChild in child)
                                {
                                    string[] nameDirMyChild = filename.Split(new Char[] { '/' });
                                    HttpContent newPhoto = GetPhotoFile(nameDirMyChild[0], myChild.InnerText);
                                    if (newPhoto != null)
                                    {
                                        listFilesName.Add(nameDirMyChild[0] + "/" + myChild.InnerText);              //enlista los archivos xml
                                        
                                        multipart.Add(newPhoto, myChild.Name, myChild.InnerText);
                                    }
                                    else
                                    {
                                        log.SaveToFile("The file" + myChild.InnerText + " does not exist, This files can not be uploaded yet");
                                        Thread.Sleep(2000);
                                        return;
                                    }
                                }
                                break;
                            default:
                                //Add a normal child:
                                log.SaveToFile("CHild in file=" + child.Name + ", value=" + child.InnerText);
                                multipart.Add(new StringContent(child.InnerText), child.Name);
                                break;
                        }
                    }
                }
                else
                {
                    log.SaveToFile("The file does not have child nodes=" + filename);
                    MoveFileToFolder(filename, FolderName_WrongFiles);
                    return;
                }
            }
            else
            {
                log.SaveToFile("The file does not exist=" + filename);
                return;
            }

            //If AdditionalParameters are specified, they always will be uploaded:
            foreach (Parameter par in service.parameters)                   //si en la lista de nodos se encuenntra un c¡archivo con algun caracter especial se subira de todas formas
            {
                switch (par.parameterType)
                {
                    case ParameterType.String:
                    case ParameterType.Int:
                    case ParameterType.Float:
                        multipart.Add(new StringContent(par.value), par.name);
                        break;
                }
            }

            try
            {
                
                HttpResponseMessage response = await client.PostAsync(serverURL + service.service, multipart);     //Respuestas del servidor

                if (response.IsSuccessStatusCode)
                {
                    StreamReader sreader = new StreamReader(await response.Content.ReadAsStreamAsync());
                    string sresponse = sreader.ReadToEnd();
                    
                    if (sresponse != "")
                    {
                        log.SaveToFile(sresponse);

                        JSONNode nodes = JSON.Parse(sresponse);

                        if (nodes["status"] != null)
                        {

                            if (nodes["status"].AsInt == 0)
                            {
                                log.SaveToFile(filename + " uploaded");
                                MoveFileToFolder(filename, FolderName_BackUp);

                                for (int i = 0; i < listFilesName.Count; i++)
                                {
                                    MoveFileToFolder(listFilesName[i], FolderName_BackUp);
                                }
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                log.SaveToFile("Service error. Status different from 0:");

                                log.SaveToFile("Error uploading " + filename + " to: " + client.BaseAddress + " Error Code: " + sresponse);
                                log.SaveToFile(sresponse);


                                if (nodes["error"] != null && nodes["error"] != "")
                                {
                                    log.SaveToFile(nodes["error"]);
                                }
                                else
                                {
                                    log.SaveToFile("Service do not return the 'error' node inside JSON");
                                }
                                Thread.Sleep(2000);
                            }
                        }
                        else
                        {
                            log.SaveToFile("Service do not return the 'status' node inside JSON");
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        log.SaveToFile("Service return an empty response");
                        Thread.Sleep(2000);
                    }
                }
                else
                {

                    log.SaveToFile("Status code is not successful");
                    log.SaveToFile(response.ToString());
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                log.SaveToFile("!!!!!EXCEPTION!!!!");
                log.SaveToFile(ex);
                throw ex;
            }
        }

        public static void MoveFileToFolder(string filename, string nameNewFolder)
        {
            string[] name = filename.Split(new Char[] { '/' });
            string[] extension = name[1].Split('.');

            log.SaveToFile("MoveFileToFolder =" + nameNewFolder);

            //Check if folder is already created.
            if (!Directory.Exists(name[0] + "/" + nameNewFolder))
                Directory.CreateDirectory(name[0] + "/" + nameNewFolder);

            string newPath = name[0] + "/" + nameNewFolder + "/" + name[1];
            if (!File.Exists(newPath))
                File.Move(filename, newPath);
            else
            {
                bool canMove = false;
                do
                {
                    Random rand = new Random(Environment.TickCount);
                    string newPathRepeated = name[0] + "/" + nameNewFolder + "/" + extension[0] + "_repeated_" + rand.Next() + "." + extension[1];
                    if (File.Exists(newPathRepeated))
                        canMove = false;
                    else
                    {
                        canMove = true;
                        File.Move(filename, newPathRepeated);
                    }
                } while (!canMove);
            }

        }

        static HttpContent GetPhotoFile(string filename, string photoName)
        {
            HttpContent fileBytes = null;

            string[] name = filename.Split(new Char[] { '/' });

            //Check if folder is already created.
            if (!Directory.Exists(name[0]))
                Directory.CreateDirectory(name[0]);

            string newPath = name[0] + "/" + photoName;
            if (File.Exists(newPath))
            {
                fileBytes = new ByteArrayContent(File.ReadAllBytes(newPath));
            }

            return fileBytes;
        }

    }
}
