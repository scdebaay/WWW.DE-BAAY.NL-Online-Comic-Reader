<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:debaay="http://www.de-baay.nl/ComicCloud"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="requestUrl"/>
  <xsl:param name="apiLocation"/>

  <!--Content table generation -->

  <xsl:template match="/">
    <div id="ComicTable">
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="debaay:file">
    <div class="ComicCell">
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="$requestUrl"/>
          <xsl:text>Comic/Read</xsl:text>
          <xsl:value-of select="@path"/>
          <xsl:text>/0</xsl:text>
        </xsl:attribute>
        <p class="ComicThumbnail">
          <img alt="thumbnail">
            <xsl:attribute name="src">
              <xsl:value-of select="$apiLocation"/>
              <xsl:text>?file=</xsl:text>
              <xsl:value-of select="@path"/>
              <xsl:text>&amp;page=0&amp;size=150</xsl:text>
            </xsl:attribute>
          </img>
        </p>
        <p class="ComicLink">
          <xsl:value-of select="@name"/>
        </p>
      </a>
    </div>
  </xsl:template>

</xsl:stylesheet>
