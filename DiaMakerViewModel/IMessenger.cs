using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiaMakerViewModel
{
    // Summary:
    //     The Messenger is a class allowing objects to exchange messages.
    public interface IMessenger
    {
        // Summary:
        //     Registers a recipient for a type of message TMessage. The action parameter
        //     will be executed when a corresponding message is sent.
        //     Registering a recipient does not create a hard reference to it, so if this
        //     recipient is deleted, no memory leak is caused.
        //
        // Parameters:
        //   recipient:
        //     The recipient that will receive the messages.
        //
        //   action:
        //     The action that will be executed when a message of type TMessage is sent.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that the recipient registers for.
        void Register<TMessage>(object recipient, Action<TMessage> action);
        //
        // Summary:
        //     Registers a recipient for a type of message TMessage.  The action parameter
        //     will be executed when a corresponding message is sent. See the receiveDerivedMessagesToo
        //     parameter for details on how messages deriving from TMessage (or, if TMessage
        //     is an interface, messages implementing TMessage) can be received too.
        //     Registering a recipient does not create a hard reference to it, so if this
        //     recipient is deleted, no memory leak is caused.
        //
        // Parameters:
        //   recipient:
        //     The recipient that will receive the messages.
        //
        //   receiveDerivedMessagesToo:
        //     If true, message types deriving from TMessage will also be transmitted to
        //     the recipient. For example, if a SendOrderMessage and an ExecuteOrderMessage
        //     derive from OrderMessage, registering for OrderMessage and setting receiveDerivedMessagesToo
        //     to true will send SendOrderMessage and ExecuteOrderMessage to the recipient
        //     that registered.
        //     Also, if TMessage is an interface, message types implementing TMessage will
        //     also be transmitted to the recipient. For example, if a SendOrderMessage
        //     and an ExecuteOrderMessage implement IOrderMessage, registering for IOrderMessage
        //     and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        //     and ExecuteOrderMessage to the recipient that registered.
        //
        //   action:
        //     The action that will be executed when a message of type TMessage is sent.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that the recipient registers for.
        void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action);
        //
        // Summary:
        //     Registers a recipient for a type of message TMessage.  The action parameter
        //     will be executed when a corresponding message is sent. See the receiveDerivedMessagesToo
        //     parameter for details on how messages deriving from TMessage (or, if TMessage
        //     is an interface, messages implementing TMessage) can be received too.
        //     Registering a recipient does not create a hard reference to it, so if this
        //     recipient is deleted, no memory leak is caused.
        //
        // Parameters:
        //   recipient:
        //     The recipient that will receive the messages.
        //
        //   token:
        //     A token for a messaging channel. If a recipient registers using a token,
        //     and a sender sends a message using the same token, then this message will
        //     be delivered to the recipient. Other recipients who did not use a token when
        //     registering (or who used a different token) will not get the message. Similarly,
        //     messages sent without any token, or with a different token, will not be delivered
        //     to that recipient.
        //
        //   action:
        //     The action that will be executed when a message of type TMessage is sent.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that the recipient registers for.
        void Register<TMessage>(object recipient, object token, Action<TMessage> action);
        //
        // Summary:
        //     Registers a recipient for a type of message TMessage.  The action parameter
        //     will be executed when a corresponding message is sent. See the receiveDerivedMessagesToo
        //     parameter for details on how messages deriving from TMessage (or, if TMessage
        //     is an interface, messages implementing TMessage) can be received too.
        //     Registering a recipient does not create a hard reference to it, so if this
        //     recipient is deleted, no memory leak is caused.
        //
        // Parameters:
        //   recipient:
        //     The recipient that will receive the messages.
        //
        //   token:
        //     A token for a messaging channel. If a recipient registers using a token,
        //     and a sender sends a message using the same token, then this message will
        //     be delivered to the recipient. Other recipients who did not use a token when
        //     registering (or who used a different token) will not get the message. Similarly,
        //     messages sent without any token, or with a different token, will not be delivered
        //     to that recipient.
        //
        //   receiveDerivedMessagesToo:
        //     If true, message types deriving from TMessage will also be transmitted to
        //     the recipient. For example, if a SendOrderMessage and an ExecuteOrderMessage
        //     derive from OrderMessage, registering for OrderMessage and setting receiveDerivedMessagesToo
        //     to true will send SendOrderMessage and ExecuteOrderMessage to the recipient
        //     that registered.
        //     Also, if TMessage is an interface, message types implementing TMessage will
        //     also be transmitted to the recipient. For example, if a SendOrderMessage
        //     and an ExecuteOrderMessage implement IOrderMessage, registering for IOrderMessage
        //     and setting receiveDerivedMessagesToo to true will send SendOrderMessage
        //     and ExecuteOrderMessage to the recipient that registered.
        //
        //   action:
        //     The action that will be executed when a message of type TMessage is sent.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that the recipient registers for.
        void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action);
        //
        // Summary:
        //     Sends a message to registered recipients. The message will reach all recipients
        //     that registered for this message type using one of the Register methods.
        //
        // Parameters:
        //   message:
        //     The message to send to registered recipients.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that will be sent.
        void Send<TMessage>(TMessage message);
        //
        // Summary:
        //     Sends a message to registered recipients. The message will reach only recipients
        //     that registered for this message type using one of the Register methods,
        //     and that are of the targetType.
        //
        // Parameters:
        //   message:
        //     The message to send to registered recipients.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that will be sent.
        //
        //   TTarget:
        //     The type of recipients that will receive the message. The message won't be
        //     sent to recipients of another type.
        void Send<TMessage, TTarget>(TMessage message);
        //
        // Summary:
        //     Sends a message to registered recipients. The message will reach only recipients
        //     that registered for this message type using one of the Register methods,
        //     and that are of the targetType.
        //
        // Parameters:
        //   message:
        //     The message to send to registered recipients.
        //
        //   token:
        //     A token for a messaging channel. If a recipient registers using a token,
        //     and a sender sends a message using the same token, then this message will
        //     be delivered to the recipient. Other recipients who did not use a token when
        //     registering (or who used a different token) will not get the message. Similarly,
        //     messages sent without any token, or with a different token, will not be delivered
        //     to that recipient.
        //
        // Type parameters:
        //   TMessage:
        //     The type of message that will be sent.
        void Send<TMessage>(TMessage message, object token);
        //
        // Summary:
        //     Unregisters a message recipient for a given type of messages only. After
        //     this method is executed, the recipient will not receive messages of type
        //     TMessage anymore, but will still receive other message types (if it registered
        //     for them previously).
        //
        // Parameters:
        //   recipient:
        //     The recipient that must be unregistered.
        //
        // Type parameters:
        //   TMessage:
        //     The type of messages that the recipient wants to unregister from.
        void Unregister<TMessage>(object recipient);
        //
        // Summary:
        //     Unregisters a messager recipient completely. After this method is executed,
        //     the recipient will not receive any messages anymore.
        //
        // Parameters:
        //   recipient:
        //     The recipient that must be unregistered.
        void Unregister(object recipient);
        //
        // Summary:
        //     Unregisters a message recipient for a given type of messages and for a given
        //     action. Other message types will still be transmitted to the recipient (if
        //     it registered for them previously). Other actions that have been registered
        //     for the message type TMessage and for the given recipient (if available)
        //     will also remain available.
        //
        // Parameters:
        //   recipient:
        //     The recipient that must be unregistered.
        //
        //   action:
        //     The action that must be unregistered for the recipient and for the message
        //     type TMessage.
        //
        // Type parameters:
        //   TMessage:
        //     The type of messages that the recipient wants to unregister from.
        void Unregister<TMessage>(object recipient, Action<TMessage> action);
        //
        // Summary:
        //     Unregisters a message recipient for a given type of messages only and for
        //     a given token. After this method is executed, the recipient will not receive
        //     messages of type TMessage anymore with the given token, but will still receive
        //     other message types or messages with other tokens (if it registered for them
        //     previously).
        //
        // Parameters:
        //   recipient:
        //     The recipient that must be unregistered.
        //
        //   token:
        //     The token for which the recipient must be unregistered.
        //
        // Type parameters:
        //   TMessage:
        //     The type of messages that the recipient wants to unregister from.
        void Unregister<TMessage>(object recipient, object token);
        //
        // Summary:
        //     Unregisters a message recipient for a given type of messages, for a given
        //     action and a given token. Other message types will still be transmitted to
        //     the recipient (if it registered for them previously). Other actions that
        //     have been registered for the message type TMessage, for the given recipient
        //     and other tokens (if available) will also remain available.
        //
        // Parameters:
        //   recipient:
        //     The recipient that must be unregistered.
        //
        //   token:
        //     The token for which the recipient must be unregistered.
        //
        //   action:
        //     The action that must be unregistered for the recipient and for the message
        //     type TMessage.
        //
        // Type parameters:
        //   TMessage:
        //     The type of messages that the recipient wants to unregister from.
        void Unregister<TMessage>(object recipient, object token, Action<TMessage> action);
    }
}
