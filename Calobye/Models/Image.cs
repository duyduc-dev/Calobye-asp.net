using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Calobye.Models
{
	public class Image
	{
		public Image()
		{
			this.image = null;
			this.path = null;
			this.fileName = null;
			this.acceptSave = true;
		}

		public Image(string path)
		{
			this.image = null;
			this.path = path;
			this.fileName = null;
			this.acceptSave = true;
		}

		public Image(string path, string filename)
		{
			this.path = path;
			this.fileName = filename;
			this.acceptSave = true;
			this.image = null;
		}

		public Image(HttpPostedFileBase image)
		{
			this.image = image;
			this.acceptSave = true;
			this.path = null;
			this.fileName = null;
		}

		public Image(HttpPostedFileBase image, string path)
		{
			this.image = image;
			this.path = path;
			this.acceptSave = true;
			this.fileName = null;
		}

		public Image(HttpPostedFileBase image, string path, string filename)
		{
			this.image = image;
			this.path = path;
			this.fileName = filename;
			this.acceptSave = true;
		}

		public Image(HttpPostedFileBase image, string path, string filename, bool accepSave)
		{
			this.image = image;
			this.path = path;
			this.fileName = filename;
			this.acceptSave = accepSave;
		}

		public HttpPostedFileBase image { get; set; }

		public string path { set; get; }

		public string fileName { get; set; }

		public bool acceptSave { set; get; }

		public ServiceResult<string> save(bool acceptSave)
		{
			return this.save(this.path, this.fileName, acceptSave);
		}

		public ServiceResult<string> save(string fileName, bool acceptSave)
		{
			return this.save(this.path, fileName, acceptSave);
		}

		public ServiceResult<string> save(string fileName)
		{
			return this.save(this.path, fileName, true);
		}

		public ServiceResult<string> save(string path, string fileName, bool acceptSave)
		{
			if (acceptSave)
			{
				if (String.IsNullOrEmpty(fileName))
				{
					return new ServiceResult<string>
					{
						message = "File name is empty.",
						data = null
					};
				}
				else if (this.image != null && this.image.ContentLength > 0)
				{
					try
					{
						string typeFile = Path.GetExtension(image.FileName).Replace(".", "");
						string _path = Path.Combine(HttpContext.Current.Server.MapPath(path),
																			 Path.GetFileName($"{fileName}.{typeFile}"));
						image.SaveAs(_path);
						return new ServiceResult<string>
						{
							message = "Upload success.",
							data = $"{fileName}.{typeFile}"
						};
					}
					catch (Exception ex)
					{
						return new ServiceResult<string>
						{
							message = "ERROR:" + ex.Message.ToString(),
							data = null
						};
					}
				}

				return new ServiceResult<string>
				{
					message = "You have not specified a file.",
					data = null
				};
			}

			return new ServiceResult<string>
			{
				message = "Not allowed to save.",
				data = null
			};
		}


		public void delete(string path, string fileName)
		{
			var imgFile = Path.Combine(HttpContext.Current.Server.MapPath(path), Path.GetFileName(fileName));
			FileInfo fi = new FileInfo(imgFile);
			if (fi != null)
			{
				File.Delete(imgFile);
				fi.Delete();
			}
		}

		public void delete(string fileName)
		{
			delete(this.path, fileName);
		}

		public void delete()
		{
			delete(this.fileName);
		}
	}
}