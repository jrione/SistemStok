Imports Amazon.S3
Imports Amazon.S3.Model
Imports Org.BouncyCastle.Math.EC.ECCurve
Imports System.IO
Imports System.Security.AccessControl
Public Class ObjectStorage
    Private bucketName As String = "sistemstok-cdn"
    Private accessKey As String = "W0oUjKKT-5TiGU0yGseb_fT9M1xggOGdtTXOyneD"
    Private secretKey As String = "WuuFXjAxoOkp9_my14bCThVLEFj3KX7y12iPas5H"
    Private serviceUrl As String = "mos.ap-southeast-3.sufybkt.com"
    Private cfg As New AmazonS3Config

    Public Sub Main()

        Try
            cfg = New AmazonS3Config() With {
                .ServiceURL = serviceUrl,
                .ForcePathStyle = True
            }

        Catch ex As Exception
            MsgBox($"Error: {ex.Message}")
        End Try

    End Sub

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
    Public Async Function UploadObject(imgName As String) As Task(Of String)
        Using s3Client As New AmazonS3Client(accessKey, secretKey, cfg)
            Dim putRequest As New PutObjectRequest() With {
                .BucketName = bucketName,
                .Key = imgName,
                .FilePath = "assets/",
                .ContentType = "image/jpeg"
            }

            Dim response As PutObjectResponse = Await s3Client.PutObjectAsync(putRequest)
            Console.WriteLine($"File '{imgName}' berhasil diunggah ke bucket '{bucketName}'.")
        End Using
    End Function

End Class
