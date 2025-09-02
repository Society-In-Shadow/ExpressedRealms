# Github and Verified Commits
Github has a feature called Verified Commits.  They work by linking a GPG key from your local machine to a public key you
upload to your account on there.  It's basically a very strong way of taking ownership of that commit in a secure manner.

It's mainly there to verify that you are in fact the one that made a commit, not someone who has the same git name and email
as you.

## Generating the Public and Private Key locally
I will caveat that these steps should be compatible across both fedora and ubuntu, but was only tested in Ubuntu.

For an up to date steps, look here : [Generating GPG Key](https://docs.github.com/en/authentication/managing-commit-signature-verification/generating-a-new-gpg-key)

## Quick Guide
First thing you want to do is generate the key, with the command below, use the information associated with your git
account.  It will ask you for some information, accept defaults where possible.

When it asks for a passphrase, make sure to keep track of it, you will need it for later.
Specifically, when using Rider, it will prompt you for your passphrase when you try committing.
It should save it after asking once, but milage may vary

```shell
gpg --full-generate-key
```

### Updating Github Account
Now you need to get the public key for github, so run the following:

```shell
gpg --list-secret-keys --keyid-format=long
```

And grab the section mentioned below on the 'sec' line

```
/Users/hubot/.gnupg/secring.gpg
------------------------------------
sec   4096R/3AA5C34371567BD2 2016-03-10 [expires: 2017-03-10]
            ^  grab this   ^
uid                          Hubot <hubot@example.com>
ssb   4096R/4BB6D45482678BE3 2016-03-10
```

Take that, and get the public key by using this command, replace with your version of the identifier.

```shell
gpg --armor --export 3AA5C34371567BD2
```

The output should have a begin block and end block, eg somethign similar to ---begin---, ---end---.  you want to copy the
whole thing including those sections as is.

Then you want to open up this link here : [Add GPG Key to Github](https://github.com/settings/keys)

There should be a GPG keys section, hit the New GPG key, and paste the output from above into that, and give it an appropriate name

## Configure Git Locally
Now that Github has the public key, you need to tell your local git instance to sign with the private key.

To do so, run the following commands
```shell
git config --global --unset gpg.format
```

Just like above, we need to grab that identifier to tell git to use the private key

So, same command as before, and grab that chunk as shown below.
```shell
gpg --list-secret-keys --keyid-format=long
```

```
/Users/hubot/.gnupg/secring.gpg
------------------------------------
sec   4096R/3AA5C34371567BD2 2016-03-10 [expires: 2017-03-10]
            ^  grab this   ^
uid                          Hubot <hubot@example.com>
ssb   4096R/4BB6D45482678BE3 2016-03-10
```

Then tell git to use that when signing the key

```shell
git config --global commit.gpgsign true
git config --global user.signingkey <keyhere>
```

## Additional Stuff
Since you are here, make sure to fill in your username and email address for git

```shell
git config --global user.name <username>
```
```shell
git config --global user.email <githubemail@example.com>
```

When using rider, it will prompt you for your passphrase when you try committing.