using EasyETL.Actions;
using EasyETL.Endpoint;
using EasyETL.Writers;
using EasyETL.Xml.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Configuration
{
    public static class EasyETLEnvironment
    {
        private static List<ClassMapping> AvailableActions = null;
        private static List<ClassMapping> AvailableWriters = null;
        private static List<ClassMapping> AvailableEndpoints = null;
        private static List<ClassMapping> AvailableParsers = null;
        private static List<ClassMapping> AvailableDatasources = null;

        public static List<ClassMapping> Actions { 
            get {
                if (AvailableActions == null) AvailableActions = ReflectionUtils.LoadClassesFromLibrary(typeof(AbstractEasyAction)).ToList();
                return AvailableActions;
            } 
        }

        public static List<ClassMapping> Writers
        {
            get
            {
                if (AvailableWriters == null) AvailableWriters = ReflectionUtils.LoadClassesFromLibrary(typeof(DatasetWriter)).ToList();
                return AvailableWriters;
            }
        }
        public static List<ClassMapping> Endpoints
        {
            get
            {
                if (AvailableEndpoints == null) AvailableEndpoints = ReflectionUtils.LoadClassesFromLibrary(typeof(AbstractEasyEndpoint)).ToList();
                return AvailableEndpoints;
            }
        }
        public static List<ClassMapping> Parsers
        {
            get
            {
                if (AvailableParsers == null) AvailableParsers = ReflectionUtils.LoadClassesFromLibrary(typeof(AbstractEasyParser)).ToList();
                return AvailableParsers;
            }
        }
        public static List<ClassMapping> Datasources
        {
            get
            {
                if (AvailableDatasources == null) AvailableDatasources = ReflectionUtils.LoadClassesFromLibrary(typeof(DatabaseEasyParser)).ToList();
                return AvailableDatasources;
            }
        }
    }
}
