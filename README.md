# Divine Inject

DivineInject is a .net dependency injection framework, designed to be simple to use and easy to understand.

## Why?

Because dependency injection is important - but done wrong it can do more harm than good.
DivineInject is opinionated about the right way to use dependency injection:

* Constructor injection or death

  Setter injection is bad for your health, just say no

* Dependencies are singletons

  Dependencies are external to your application - your DI framework doesn't need to know about users or sessions or threads.
  
* Domain objects can be rich, too

  Your domain model doesn't have to be [anemic](https://en.wikipedia.org/wiki/Anemic_domain_model)
  
## Getting Started

First, install the nuget package:

```
Install-Package DivineInject
```

Second, wire up your dependencies:

```
DivineInjector.Current
	.Bind<IDatabaseProvider>().To<MSSQLDatabaseProvider>()
	.Bind<IOrderService>().To<OrderService>();
```
  
Third, create your root object:

```
public object GetInstance(InstanceContext instanceContext, Message message)
{
    return DivineInjector.Current.Get(m_instanceType);
}
```
  
## Documentation

For more documentation and detailed examples, see the [documentation](http://divineinject.github.io/).
