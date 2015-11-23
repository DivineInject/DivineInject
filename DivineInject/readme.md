# Divine Inject

DivineInject is a .net dependency injection framework, designed to be simple to use and easy to understand.

## Why?

Because dependency injection is important - but done wrong it can do more harm than good.
DivineInject is opinionated about the right way to use dependency injection:

* Constructor injection or death

  No, setter injection is bad for your health, just say no

* Dependencies are singletons

  Dependencies are external to your application - your DI framework doesn't need to know about users or sessions or threads.
  
* Domain objects can be rich, too

  Your domain model doesn't have to be [anemic](https://en.wikipedia.org/wiki/Anemic_domain_model)
  
### Constructor Injection

Setter and method injection are much harder to get right - so DivineInject simply doesn't support them. If you can't implement your dependencies as 
constructor arguments, then maybe you should refactor the dependency so you can.

### Singleton Dependencies

All dependency injection frameworks get wrapped up in different scopes. These are almost always confusing and open to abuse - so DivineInject
simply doesn't support them. With DivineInject all dependencies must be singletons. If you need a user- or session- or thread-specific dependency,
then implement the dependency provider in your own code. It's not hard to write, but it's a lot easier to debug when the code is under your control.

### Rich Domain Objects

DivineInject borrows an idea from [Google Guice](https://github.com/google/guice) - with Guice it is called "assisted injection", in
DivineInject we call it generated factory injection. The idea is the same - providing a simple way to create objects with constructors
which accept runtime arguments as well as dependencies to inject. This allows you to create rich, stateful domain objects which also have
dependencies.
