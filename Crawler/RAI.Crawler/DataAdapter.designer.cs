﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.237
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RAI.Crawler.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="db")]
	public partial class DataAdapterDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    partial void InsertDataUri(DataUri instance);
    partial void UpdateDataUri(DataUri instance);
    partial void DeleteDataUri(DataUri instance);
    #endregion
		
		public DataAdapterDataContext() : 
				base(global::RAI.Crawler.Properties.Settings.Default.dbConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataAdapterDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAdapterDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAdapterDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataAdapterDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<DataUri> DataUri
		{
			get
			{
				return this.GetTable<DataUri>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Uri")]
	public partial class DataUri : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _AbsoluteUri;
		
		private System.Nullable<int> _Parent;
		
		private string _Cache;
		
		private System.Nullable<bool> _Status;
		
		private EntitySet<DataUri> _DataUri1;
		
		private EntityRef<DataUri> _ParentUri;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnAbsoluteUriChanging(string value);
    partial void OnAbsoluteUriChanged();
    partial void OnParentChanging(System.Nullable<int> value);
    partial void OnParentChanged();
    partial void OnCacheChanging(string value);
    partial void OnCacheChanged();
    partial void OnStatusChanging(System.Nullable<bool> value);
    partial void OnStatusChanged();
    #endregion
		
		public DataUri()
		{
			this._DataUri1 = new EntitySet<DataUri>(new Action<DataUri>(this.attach_DataUri1), new Action<DataUri>(this.detach_DataUri1));
			this._ParentUri = default(EntityRef<DataUri>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AbsoluteUri", DbType="VarChar(1000) NOT NULL", CanBeNull=false)]
		public string AbsoluteUri
		{
			get
			{
				return this._AbsoluteUri;
			}
			set
			{
				if ((this._AbsoluteUri != value))
				{
					this.OnAbsoluteUriChanging(value);
					this.SendPropertyChanging();
					this._AbsoluteUri = value;
					this.SendPropertyChanged("AbsoluteUri");
					this.OnAbsoluteUriChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Parent", DbType="Int")]
		public System.Nullable<int> Parent
		{
			get
			{
				return this._Parent;
			}
			set
			{
				if ((this._Parent != value))
				{
					if (this._ParentUri.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnParentChanging(value);
					this.SendPropertyChanging();
					this._Parent = value;
					this.SendPropertyChanged("Parent");
					this.OnParentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Cache", DbType="Char(11)")]
		public string Cache
		{
			get
			{
				return this._Cache;
			}
			set
			{
				if ((this._Cache != value))
				{
					this.OnCacheChanging(value);
					this.SendPropertyChanging();
					this._Cache = value;
					this.SendPropertyChanged("Cache");
					this.OnCacheChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="Bit")]
		public System.Nullable<bool> Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Uri_Uri", Storage="_DataUri1", ThisKey="Id", OtherKey="Parent")]
		public EntitySet<DataUri> DataUri1
		{
			get
			{
				return this._DataUri1;
			}
			set
			{
				this._DataUri1.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Uri_Uri", Storage="_ParentUri", ThisKey="Parent", OtherKey="Id", IsForeignKey=true)]
		public DataUri ParentUri
		{
			get
			{
				return this._ParentUri.Entity;
			}
			set
			{
				DataUri previousValue = this._ParentUri.Entity;
				if (((previousValue != value) 
							|| (this._ParentUri.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._ParentUri.Entity = null;
						previousValue.DataUri1.Remove(this);
					}
					this._ParentUri.Entity = value;
					if ((value != null))
					{
						value.DataUri1.Add(this);
						this._Parent = value.Id;
					}
					else
					{
						this._Parent = default(Nullable<int>);
					}
					this.SendPropertyChanged("ParentUri");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_DataUri1(DataUri entity)
		{
			this.SendPropertyChanging();
			entity.ParentUri = this;
		}
		
		private void detach_DataUri1(DataUri entity)
		{
			this.SendPropertyChanging();
			entity.ParentUri = null;
		}
	}
}
#pragma warning restore 1591
