using System;
using System.Reflection;
using System.Reflection.Emit;
using MiNET.Net;

namespace MiNET.Plugins
{
    public interface IPackageEventHandler
    {
        Package Handle(Package package, Player player);
    }

    public interface IPackageEventHandlerGenerator
    {
        IPackageEventHandler Generate(object plugin, MethodInfo method, Type type);
    }

    public class ReflectionPackageHandlerGenerator : IPackageEventHandlerGenerator
    {
        private sealed class ReflectionPackageEventHandler : IPackageEventHandler
        {
            private readonly Func<Package, Player, Package> @delegate;

            public ReflectionPackageEventHandler(Func<Package, Player, Package> @delegate)
            {
                this.@delegate = @delegate;
            }

            public Package Handle(Package package, Player player)
            {
                return @delegate.Invoke(package, player);
            }
        }

        public IPackageEventHandler Generate(object plugin, MethodInfo method, Type type)
        {
            if (method.IsStatic)
            {
                return new ReflectionPackageEventHandler((package, player) =>
                {
                    method.Invoke(null, new object[] { package, player });
                    return package;
                });
            }

            if (method.GetParameters().Length == 2)
            {
                return new ReflectionPackageEventHandler((package, player) => (Package)method.Invoke(plugin, new object[] { package, player }));
            }
            return new ReflectionPackageEventHandler((package, player) => (Package)method.Invoke(plugin, new object[] { package }));
        }
    }

    public class EmittedPackageHandlerGenerator : IPackageEventHandlerGenerator
    {
        private static readonly TypeAttributes SharedExecutorTypeAttributes = TypeAttributes.Public |
                                                                        TypeAttributes.AutoClass |
                                                                        TypeAttributes.AnsiClass |
                                                                        TypeAttributes.BeforeFieldInit;

        private static readonly MethodAttributes SharedExecutorConstructorAttributes = MethodAttributes.Public |
                                                                                       MethodAttributes.HideBySig |
                                                                                       MethodAttributes.RTSpecialName |
                                                                                       MethodAttributes.SpecialName;

        private static readonly MethodAttributes SharedExecutorMethodAttributes = MethodAttributes.Public |
                                                                                        MethodAttributes.HideBySig |
                                                                                        MethodAttributes.NewSlot |
                                                                                        MethodAttributes.Virtual |
                                                                                        MethodAttributes.Final;

        private static readonly FieldAttributes InstanceExecutorFieldAttributes = FieldAttributes.Private |
                                                                                  FieldAttributes.InitOnly;

        private static readonly Type[] ExecutorInterfaces = { typeof(IPackageEventHandler) };

        private static readonly ConstructorInfo ObjectConstructorInfo = typeof(object).GetConstructor(new Type[0]);

        public IPackageEventHandler Generate(object plugin, MethodInfo method, Type packageType)
        {
            if (!method.DeclaringType?.IsVisible ?? true)
            {
                // TODO: Figure out more appropriate method of handling.
                return new NopPackageEventHandler();
            }

            string name = Guid.NewGuid().ToString().Replace("-", "");

            AssemblyName assemblyName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(string.Format("MiNET.Plugins.Executor_{0}", name), SharedExecutorTypeAttributes, typeof(object), ExecutorInterfaces);

            object[] constructorArguments;
            if (method.IsStatic)
            {
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(SharedExecutorConstructorAttributes, CallingConventions.Standard, Type.EmptyTypes);
                constructorBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
                ILGenerator generator = constructorBuilder.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Call, ObjectConstructorInfo);
                generator.Emit(OpCodes.Ret);

                MethodBuilder methodBuilder = typeBuilder.DefineMethod("Handle", SharedExecutorMethodAttributes, CallingConventions.Standard, typeof(Package), new[] { typeof(Package), typeof(Player) });
                methodBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
                generator = methodBuilder.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Isinst, packageType);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, method);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ret);

                constructorArguments = new object[0];
            }
            else
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField("_delegate", plugin.GetType(), InstanceExecutorFieldAttributes);
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(SharedExecutorConstructorAttributes, CallingConventions.Standard, new[] { plugin.GetType() });
                constructorBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
                ILGenerator generator = constructorBuilder.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Call, ObjectConstructorInfo);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Stfld, fieldBuilder);
                generator.Emit(OpCodes.Ret);

                MethodBuilder methodBuilder = typeBuilder.DefineMethod("Handle", SharedExecutorMethodAttributes, CallingConventions.HasThis, typeof(Package), new[] { typeof(Package), typeof(Player) });
                methodBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
                generator = methodBuilder.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldBuilder);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Isinst, packageType);
                if (method.GetParameters().Length == 2)
                {
                    generator.Emit(OpCodes.Ldarg_2);
                }
                generator.Emit(OpCodes.Callvirt, method);
                if (method.ReturnType == typeof(void))
                {
                    generator.Emit(OpCodes.Ldarg_1);
                }
                generator.Emit(OpCodes.Ret);

                constructorArguments = new[] { plugin };
            }

            Type result = typeBuilder.CreateType();
            return (IPackageEventHandler)Activator.CreateInstance(result, constructorArguments);
        }
    }

    internal class NopPackageEventHandler : IPackageEventHandler
    {
        public Package Handle(Package package, Player player)
        {
            return package;
        }
    }
}
