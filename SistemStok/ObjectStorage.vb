Imports Amazon.S3
Imports Amazon.S3.Model
Imports Org.BouncyCastle.Math.EC.ECCurve
Imports System.IO
Imports System.Net.Mime
Imports System.Security.AccessControl
Public Class ObjectStorage
    Private bucketName As String = "sistemstok-cdn"
    Private accessKey As String = "W0oUjKKT-5TiGU0yGseb_fT9M1xggOGdtTXOyneD"
    Private secretKey As String = "WuuFXjAxoOkp9_my14bCThVLEFj3KX7y12iPas5H"
    Private serviceUrl As String = "https://mos.ap-southeast-3.sufybkt.com"
    Private cfg As AmazonS3Config
    Private Function GetContentType(fileExtension As String) As String
        Select Case fileExtension.ToLower()
            Case ".jpg", ".jpeg"
                Return "image/jpeg"
            Case ".png"
                Return "image/png"
            Case ".gif"
                Return "image/gif"
            Case ".bmp"
                Return "image/bmp"
            Case Else
                Return "application/octet-stream"
        End Select
    End Function
    Public Async Function UploadObject(imagePath As String, imageName As String) As Task(Of String)
        Try
            cfg = New AmazonS3Config() With {
                .ServiceURL = serviceUrl,
                .ForcePathStyle = True
            }

        Catch ex As Exception
            MsgBox($"Error: {ex.Message}")
        End Try

        Using s3Client As New AmazonS3Client(accessKey, secretKey, cfg)

            Dim fileExtension As String = Path.GetExtension(imagePath)
            Dim contentType As String = GetContentType(fileExtension)

            Dim putRequest As New PutObjectRequest() With {
                .BucketName = bucketName,
                .Key = "assets/" & imageName,
                .FilePath = imagePath,
                .ContentType = contentType
            }

            Dim response As PutObjectResponse = Await s3Client.PutObjectAsync(putRequest)
            Console.WriteLine($"File '{imageName}' berhasil diunggah ke bucket '{bucketName}'.")
        End Using
    End Function

End Class
