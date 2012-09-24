<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output
    encoding="UTF-8"
    indent="yes"
    method="xml"
    omit-xml-declaration="yes"
  />

<xsl:template match="Page">---
layout: docs
title:  <xsl:value-of select="Title" />
---
<!-- HEADER -->
<div id="header_wrap" class="outer">
  <header class="inner">
    <a id="forkme_banner" href="https://github.com/txdv/sundown.net">View on GitHub</a>

    <!--<h1 id="project_title">Sundown.net</h1>-->
    <!--<h2 id="project_tagline">A .NET wrapper of the nice sundown markdown library.</h2>-->
    <xsl:call-template name="create-default-title" />
    <h2 id="project_tagline">
      <xsl:call-template name="create-default-summary" />
    </h2>
  </header>
</div>

<!-- MAIN CONTENT -->
<div id="main_content_wrap" class="outer">
  <section id="main_content" class="inner">
    <xsl:call-template name="create-default-collection-title" />
    <xsl:call-template name="create-index" />
    <xsl:call-template name="create-default-signature" />
    <xsl:call-template name="create-default-remarks" />
    <xsl:call-template name="create-default-members" />
  </section>
</div>
</xsl:template>

  <!-- IDENTITY TRANSFORMATION -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template name="create-index">
    <xsl:if test="
        count(PageTitle/@id) &gt; 0 and 
        count(Signature/@id) &gt; 0 and
        count(Remarks/@id) &gt; 0 and
        count(Members/@id) &gt; 0
        ">
      <div class="SideBar">
        <p>
          <a>
            <xsl:attribute name="href">
              <xsl:text>#</xsl:text>
              <xsl:value-of select="PageTitle/@id" />
            </xsl:attribute>
            <xsl:text>Overview</xsl:text>
          </a>
        </p>
        <p>
          <a>
            <xsl:attribute name="href">
              <xsl:text>#</xsl:text>
              <xsl:value-of select="Signature/@id" />
            </xsl:attribute>
            <xsl:text>Signature</xsl:text>
          </a>
        </p>
        <p>
          <a>
            <xsl:attribute name="href">
              <xsl:text>#</xsl:text>
              <xsl:value-of select="Remarks/@id" />
            </xsl:attribute>
            <xsl:text>Remarks</xsl:text>
          </a>
        </p>
        <p>
          <a href="#Members">Members</a>
        </p>
        <p>
          <a>
            <xsl:attribute name="href">
              <xsl:text>#</xsl:text>
              <xsl:value-of select="Members/@id" />
            </xsl:attribute>
            <xsl:text>Member Details</xsl:text>
          </a>
        </p>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template name="create-default-collection-title">
    <div class="CollectionTitle">
      <xsl:apply-templates select="CollectionTitle/node()" />
    </div>
  </xsl:template>

  <xsl:template name="create-default-title">
    <h1 id="project_title">
      <xsl:apply-templates select="PageTitle/node()" />
    </h1>
  </xsl:template>

  <xsl:template name="create-default-summary">
    <p class="Summary">
      <xsl:if test="count(Summary/@id) &gt; 0">
        <xsl:attribute name="id">
          <xsl:value-of select="Summary/@id" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="Summary/node()" />
    </p>
  </xsl:template>

  <xsl:template name="create-default-signature">
    <div>
      <xsl:if test="count(Signature/@id) &gt; 0">
        <xsl:attribute name="id">
          <xsl:value-of select="Signature/@id" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="Signature/node()" />
    </div>
  </xsl:template>

  <xsl:template name="create-default-remarks">
    <div class="Remarks">
      <xsl:if test="count(Remarks/@id) &gt; 0">
        <xsl:attribute name="id">
          <xsl:value-of select="Remarks/@id" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="Remarks/node()" />
    </div>
  </xsl:template>

  <xsl:template name="create-default-members">
    <div class="Members">
      <xsl:if test="count(Members/@id) &gt; 0">
        <xsl:attribute name="id">
          <xsl:value-of select="Members/@id" />
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="Members/node()" />
    </div>
  </xsl:template>

  <xsl:template name="create-default-copyright">
    <div class="Copyright">
      <xsl:apply-templates select="Copyright/node()" />
    </div>
  </xsl:template>
</xsl:stylesheet>
