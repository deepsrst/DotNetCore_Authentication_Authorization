// See https://aka.ms/new-console-template for more information

// Specify the path to your file
string filePath = "D:\\Office Projects And Files\\FileChecklist_Repo\\MIRS-1480 MYRemit Certificate changes\\attachements\\KeyClean\\PrivateKey.txt";
string outputFilePath = "D:\\Office Projects And Files\\FileChecklist_Repo\\MIRS-1480 MYRemit Certificate changes\\attachements\\KeyClean\\PrivateKeyCleaned.txt";
try
{
    // Read the RSA private key from the file
    string inputKey = File.ReadAllText(filePath);

    // Remove header and footer
    string keyWithoutHeaderFooter = inputKey.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "");

    // Remove white spaces
    string keyWithoutSpaces = new string(keyWithoutHeaderFooter.Where(c => !char.IsWhiteSpace(c)).ToArray());

    Console.WriteLine(keyWithoutSpaces);

    File.WriteAllText(outputFilePath, keyWithoutSpaces);
}
catch (Exception ex)
{
    Console.WriteLine($"Error reading the file: {ex.Message}");
}
    
