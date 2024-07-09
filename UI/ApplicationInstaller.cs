using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UI
{
    [RunInstaller(true)]
    public partial class ApplicationInstaller : System.Configuration.Install.Installer
    {
        public ApplicationInstaller()
        {
            InitializeComponent();
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }

        public override void Install(IDictionary stateSaver)
        {
            stateSaver.Add("assemblypath", Context.Parameters["assemblypath"]);

            base.Install(stateSaver);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = Path.Combine(appDataPath, "EcologicalTaxesCalculator");

            CreateBaseFolders(appFolderPath);
            CopyFiles(Path.Combine(appFolderPath, "cache"), savedState);
            
            base.OnAfterInstall(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);
        }

        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted(savedState);
        }


        private void CreateBaseFolders(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            string dataFolder = Path.Combine(path, "data");
            Directory.CreateDirectory(dataFolder);

            string cacheFolder = Path.Combine(path, "cache");
            Directory.CreateDirectory(cacheFolder);
        }

        private void CopyFiles(string targetPath, IDictionary savedState)
        {
            string assemblyPath = (string)savedState["assemblypath"];
            assemblyPath = assemblyPath.Remove(assemblyPath.Length - 1 - 6);

            string backProdsPath = Path.Combine(assemblyPath, "backedProducts.xml");
            string backProdsDest = Path.Combine(targetPath, "backedProducts.xml");
            File.Copy(backProdsPath, backProdsDest, true);

            string furnsPath = Path.Combine(assemblyPath, "furnaces.xml");
            string furnsDest = Path.Combine(targetPath, "furnaces.xml");
            File.Copy(furnsPath, furnsDest, true);

            string steamBoilsPath = Path.Combine(assemblyPath, "steamBoilers.xml");
            string steamBoilsDest = Path.Combine(targetPath, "steamBoilers.xml");
            File.Copy(steamBoilsPath, steamBoilsDest, true);

            string waterBoilsPath = Path.Combine(assemblyPath, "hotWaterBoilers.xml");
            string waterBoilsDest = Path.Combine(targetPath, "hotWaterBoilers.xml");
            File.Copy(waterBoilsPath, waterBoilsDest, true);

            string taxParamsPath = Path.Combine(assemblyPath, "taxParams.xml");
            string taxParamsDest = Path.Combine(targetPath, "taxParams.xml");
            File.Copy(taxParamsPath, taxParamsDest, true);
        }
    }
}
